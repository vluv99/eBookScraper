import { Novel } from './Novel.js';
import { sites } from './data.js';
import { Chapter } from './Chapter.js';
import { DataNode } from 'domhandler';

export class Website {
	type: string;
	url: string;
	novels: Novel[];
   
	constructor(type: string, url: string, novels: Novel[]) {
		this.type = type;
		this.url = url;
		this.novels = novels;
	}


}

export const types = {
	test: 'example',
	chrysanthemumgarden: 'chrysanthemumgarden',
	wuxiaworld: 'wuxiaworld'
};