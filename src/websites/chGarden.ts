import { DataNode } from 'domhandler';
import { Chapter } from '../Chapter.js';
import { Novel } from '../Novel.js';
import { types } from '../Website.js';
import { sites } from '../data.js';
import { loadDataIntoFile } from '../saveToFile.js';
import * as cheerio from 'cheerio';
import * as cliProgress from 'cli-progress';

export abstract class Scraper{
	abstract scrapeNovel(url: string) : Promise<Novel>
}

export class Chrysanthemumgarden extends Scraper{
	async scrapeNovel(url: string): Promise<Novel> {
		const bar1 = new cliProgress.SingleBar({}, cliProgress.Presets.shades_classic);

		const html = await loadDataIntoFile(url, types.chrysanthemumgarden + '_index.html');
		const $ = cheerio.load(html);

		const n = sites.chrysanthemumgarden.novels[0];

		// novel info
		const novelTitle = $('.novel-info h1.novel-title').contents().first().text();
		const altTitle = $('.novel-info h1.novel-title').children().last().text();
		const author = $('.novel-container .novel-info').contents().eq(4).text();
		let synopsis = '';

		const synopsisContent = $('.entry-content p');
		for(const p of synopsisContent){
			for (let i = 0; i < p.children.length; i++) {
				const d = p.children[i] as DataNode;

				if(d.data){
					synopsis += '<p>' + d.data + '</p>';
				}
			}
		}

		const chapters: string[] = [];
		$('.translated-chapters .chapter-item a').each( function(i) {
			const link:string = $(this).attr('href') ?? '';
			chapters.push(link);
		});


		bar1.start(chapters.length, 0);
		let i = 0;
		for(const item of chapters/*.slice(0, 3)*/) {
			n.content.push(await this.scrapeChapter(item));
			bar1.update(i++);
		}


		// Fill in data
		n.title = novelTitle;
		n.altTitle = altTitle;
		n.author = author;
		n.synopsis = synopsis;
		n.chapterNumber = chapters.length;

		bar1.stop();
		return n;
	}

	async scrapeChapter(url:string){
		const html = await loadDataIntoFile(url, types.chrysanthemumgarden + '_index.html');
		const $ = cheerio.load(html);
        
		const chTitle = $('h1 .chapter-title').text();
		const content = $('#novel-content p');

		const ch = new Chapter(chTitle);

		for(const p of content){
			for (let i = 0; i < p.children.length; i++) {
				const d = p.children[i] as DataNode;

				if(d.data){
					ch.content.push('<p>' + d.data + '</p>');
				}
			}
			
		}

		return ch;
	}
}