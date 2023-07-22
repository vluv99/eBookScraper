import { Novel } from '../Novel.js';
import { Website } from '../Website.js';

export abstract class Scraper{
	abstract scrapeNovel(novel: Novel, site:Website) : Promise<Novel>
}