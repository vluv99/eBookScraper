import { loadDataIntoFile, writeFile } from './saveToFile.js';
import { loadFromURL } from './load.js';
import { Website, types } from './Website.js';
import { Novel } from './Novel.js';
import { Chapter } from './Chapter.js';
import { DataNode } from 'domhandler';

import * as util from 'util';
import { sites } from './data.js';
import { Chrysanthemumgarden } from './websites/chGarden.js';

console.log('Ebook Scraper Starting up');

const isHTMLParseTestMode = false;
/*
async function parseHTML(html: string, site: Website) {
	const $ = cheerio.load(html);

	let novel, title, content;

	if (site.type === types.test) {
		return $('body');
	} else if (site.type === types.chrysanthemumgarden) {
		
}*/

const main = async function () {
	let html, content;

	const site = sites.chrysanthemumgarden;

	if (isHTMLParseTestMode) {
		html = await loadDataIntoFile(site.url, site.type + '.html');
		//content = await parseHTML(html, site);

	} else {
		const ch = new Chrysanthemumgarden();
		content = await ch.scrapeNovel(site.novels[0].url);
		content.toEPubConfig();
	}

	//writeFile('_' + site.type + '.html', content ? '' : content!.toString());
	//console.log(util.inspect(content, {showHidden: false, depth: null, colors: true}));
	
};

await main();