
import { readFileSync, writeFileSync } from 'fs';
import { join } from 'path';
import * as fs from 'fs';
import { loadFromURL } from './load.js';

const dest = './data';

export function writeFile(filename: string, data: string){
	writeFileSync(join(dest, filename), data, {
		flag: 'w',
	});
}

export async function loadDataIntoFile(url: string, filename: string){
	if(!fs.existsSync(dest+'/'+filename)){
		const html = await loadFromURL(url);
		writeFile(filename, html);
	}

	return readFileSync(join(dest, filename), 'utf-8');
}