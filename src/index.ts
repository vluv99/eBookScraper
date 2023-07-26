import 'source-map-support/register.js';

import { loadDataIntoFile, writeFile } from './saveToFile.js';
import { loadFromURL } from './load.js';
import { Website, types } from './Website.js';
import { Novel } from './Novel.js';
import { Chapter } from './Chapter.js';
import { DataNode } from 'domhandler';

import * as util from 'util';
import { sites } from './data.js';
import { Chrysanthemumgarden } from './websites/chGarden.js';
import { Wuxiaworld } from './websites/wuxiaworld.js';

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

	const site = sites.wuxiaworld;//sites.chrysanthemumgarden;

	if (isHTMLParseTestMode) {
		html = await loadDataIntoFile(site.url, site.type + '.html');

	} else {
		const ch = new Wuxiaworld();//new Chrysanthemumgarden();
		for (let i = 20; i < site.novels.length; i++) {
			console.log('\n ############################ \n');			
			content = await ch.scrapeNovel(site.novels[i], site);
			content.toEPubConfig();
		}
	}
};

await main();