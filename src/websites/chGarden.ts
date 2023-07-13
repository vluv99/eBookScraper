import { DataNode, NodeWithChildren } from 'domhandler';
import { Chapter } from '../Chapter.js';
import { Novel } from '../Novel.js';
import { types } from '../Website.js';
import { sites } from '../data.js';
import { loadDataIntoFile } from '../saveToFile.js';
import * as cheerio from 'cheerio';
import * as cliProgress from 'cli-progress';
import { ElementType } from 'domelementtype';

import he from 'he';
import { decodeJumble } from '../util/decodeJumble.js';
import { AuthorsNote } from '../AuthorsNote.js';
import { Scraper } from './Scraper.js';

export class Chrysanthemumgarden extends Scraper{
	async scrapeNovel(n: Novel): Promise<Novel> {
		const bar1 = new cliProgress.SingleBar({}, cliProgress.Presets.shades_classic);

		const html = await loadDataIntoFile(n.url, types.chrysanthemumgarden + '-' + n.ePubName + '_index.html');
		const $ = cheerio.load(html);

		//const n = sites.chrysanthemumgarden.novels[0];

		// novel info
		n.title = $('.novel-info h1.novel-title').contents().first().text();
		n.altTitle = $('.novel-info h1.novel-title').children().last().text();
		n.author = $('.novel-container .novel-info').contents().eq(4).text();
		n.contributors = $('.inside-article').contents().eq(6).text();
		n.thumbnail = $('.novel-cover img').attr('src');

		const genres = $('.novel-info .series-genres').children();
		for(const g of genres){
			const a = g.children[0] as DataNode;
			n.genres.push(a.data);
		}
        
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
		let i = 1;
		for(const item of chapters) {
			n.content.push(await this.scrapeChapter(item, i, n.ePubName));
			bar1.update(i++);
		}


		// Fill in data
		n.synopsis = synopsis;
		n.chapterCount = chapters.length;

		bar1.stop();
		return n;
	}

	async scrapeChapter(url:string, chNumber: number, ePubName: string){
		const html = await loadDataIntoFile(url, types.chrysanthemumgarden + '-' + ePubName + '_' + chNumber + '.html');
		const $ = cheerio.load(html);
        
		const chTitle = $('h1 .chapter-title').text();
		const content = $('#novel-content p');
		const tooltips = $('.tooltip-container');

		const ch = new Chapter(chTitle);

		for(const p of content){
			if(!this.isVisible(p)){
				continue;
			}

			const paragraph = this.traverse(p, '');	
			if(paragraph){
				ch.content.push('<p>' + paragraph + '</p>');
			}
		}

		const authorsNotes: AuthorsNote [] = []; // TODO: add context data to array
		if(tooltips.length > 0){
			ch.content.push('<h3>Translator\'s Note:</h3>');
			let notes = '<ol>';
			for(const tooltip of tooltips){
				const tooltip_content = this.traverse(tooltip,'').replace('Translator\'s Note','');

				const d: DataNode = tooltip.children[2] as DataNode;
				const an = new AuthorsNote(tooltip.attribs.id, tooltip_content);
				notes += `
                    <li>
                        <a style="text-decoration:none; margin-bottom: 4px;" id="${an.id}" href="#${an.id}_origin">
                        ${an.content}
                        </a>
                    </li>`;

				authorsNotes.push(an);
			}
			notes += '</ol>';
			ch.content.push(notes);
		}
		
		ch.number = chNumber;

		return ch;
	}

	isVisible(node:cheerio.Element){
		return !node.attribs.style?.includes('height:1px;');
	}

	isTooltipTitle(node:cheerio.Element){
		return node.attribs.class?.includes('tooltip-title');
	}

	traverse(node:cheerio.Node, paragraph: string, ): string{       
		if(node.type === ElementType.Tag){
			const tag = node as cheerio.Element;
			if(!this.isVisible(tag) || this.isTooltipTitle(tag)){
				return '';
			}

			for(const item of tag.childNodes){
				paragraph += this.traverse(item, '');
			}
		} else if(node.type === ElementType.Text){
			const d: DataNode = node as DataNode;
			if(d){
				if (d.parent?.type === ElementType.Tag) {
					const parent = d.parent as cheerio.Element;
					let text = d.data;
					if(parent.attribs.class?.includes('jum')){
						text = decodeJumble(text);
					}

					if(parent.name === 'span' && parent.attribs.class?.includes('tooltip')){
						paragraph += `<a id="${parent.attribs['tooltip-target']}_origin" href="#${parent.attribs['tooltip-target']}"> ${he.encode(text)} </a>`;
					} else {
						paragraph += he.encode(text);
					}
				}
			}	
		}

		return paragraph;
	}
}