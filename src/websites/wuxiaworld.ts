import { Novel } from '../Novel.js';
import { Website, types } from '../Website.js';
import { dest, loadDataIntoFile } from '../saveToFile.js';
import { Scraper } from './Scraper.js';

import * as cliProgress from 'cli-progress';
import * as cheerio from 'cheerio';
import { DataNode, NodeWithChildren } from 'domhandler';
import { Chapter } from '../Chapter.js';
import { loadFromURL } from '../load.js';
import he from 'he';
import sharp from 'sharp';
import axios from 'axios';

import * as fs from 'fs';

export class Wuxiaworld extends Scraper{
	async scrapeNovel(n: Novel, site:Website): Promise<Novel> {
		const bar1 = new cliProgress.SingleBar({}, cliProgress.Presets.shades_classic);

		const html = await loadDataIntoFile(n.url, types.wuxiaworld + '-' + n.ePubName + '_index.html');
		const $ = cheerio.load(html);

		n.title = $('.mantine-Container-root h5').first().text();

		const author = $('.mantine-Container-root h5').parent()[0].children[1] as cheerio.Element;
		n.author=  (author.children[2] as DataNode).data;

		n.thumbnail = $('.mantine-Paper-root img').first().attr('src');
		if(n.thumbnail?.includes('?')){
			const i = n.thumbnail.indexOf('?');
			n.thumbnail = n.thumbnail.substring(0, i);
			//n.thumbnail += '?width=1000&quality=100';
		}

		if(n.thumbnail?.includes('webp')){
			const response = await axios.get(n.thumbnail,  { responseType: 'arraybuffer' });
			const buf = await sharp(response.data).jpeg().toBuffer(); 
			fs.writeFileSync(dest+'/'+types.wuxiaworld + '-' + n.ePubName + '_cover.jpeg', buf);
			n.thumbnail = dest+'/'+types.wuxiaworld + '-' + n.ePubName + '_cover.jpeg';
		}

		n.synopsis = '<p>' + $('.mantine-Tabs-panel .mantine-Spoiler-content .mantine-Text-root').first().text() + '</p>';


		const a = $('.mantine-Card-root').first().children();
		const b = $(a).contents();
		$(b).each( function(i) {
			const filter = $(this).text().includes('Categories');
			if(filter){
				$('span',this).each(function (i){
					const c:string = $(this).text();
					n.tags.push(c);
				});
				/*const ab = $(this).contents()[1];
				const abc = $(ab).contents()[0];
				const abcd = $(abc).contents()[0];
				const abcde = $(abcd).contents()[0];
				const abcdef = $(abcde).contents()[0];
				const abcdefg = $(abcdef).children();

				$(abcdefg).each( function(i) {
					//const c:string = $(this).text();
					//n.tags.push(c);
				});*/
			}	
		});
		/*$('.mantine-Spoiler-root a span').each( function(i) {
			const c:string = $(this).text();
			n.tags.push(c);
		});*/


		const chapters = await loadFromURL(site.url + 'api/chapters/' + n.ePubName);
		bar1.start(chapters.length, 0);
		for(const item of chapters) {
			n.content.push(await this.scrapeChapter(site.url + 'chapter/' + item.novSlugChapSlug, item.index, n.ePubName, item.title));
			bar1.update(item.index);
		}
		n.chapterCount = chapters.length;

		bar1.stop();
		return n;
	}

	async scrapeChapter(url:string, chNumber: number, ePubName: string, title: string){
		const html = await loadDataIntoFile(url, types.wuxiaworld + '-' + ePubName + '_' + chNumber + '.html');
		const $ = cheerio.load(html);
        
		const chTitle = title;
		const content = $('.mantine-Paper-root #chapterText').contents();

		const ch = new Chapter(chTitle);

		for(const p of content){
			const d = (p as DataNode).data;
			const subscripts = /[\u2070-\u209F\u00B2\u00B3\u00B9]/g;

			if(subscripts.test(d)){
				ch.content.push('<p>' + he.encode((p as DataNode).data) + '<a href="#ch_' + chNumber + '_end"> (TN)</a>' + '</p>');
			} else{
				ch.content.push('<p>' + he.encode((p as DataNode).data) + '</p>');
			}
		}

		ch.content.push('<p id="ch_' + chNumber + '_end"></p>');
		
		ch.number = chNumber;

		return ch;
	}
}