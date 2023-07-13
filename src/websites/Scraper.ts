import { Novel } from '../Novel.js';

export abstract class Scraper{
	abstract scrapeNovel(novel: Novel) : Promise<Novel>
}