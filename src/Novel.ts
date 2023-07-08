import { Chapter } from './Chapter.js';
import epub from 'epub-gen';


export class Novel {
	url: string;
	ePubName: string;
	title =  '';
	content: Chapter[] = [];
	altTitle = '';
	author = '';
	chapterNumber = 0;
	synopsis = '';
	genres = '';
	translators = '';
	tags = '';
	thumbnail = '';

   
	constructor(url: string, ePubName:string) {
		this.url = url;
		this.ePubName = ePubName;
	}

	toEPubConfig(){
		const options = {
			title: this.title,
			author: this.author,
			output: './data/'+this.ePubName+'.epub',
			content: this.content.map(ch => ch.toEPubConfig())
		};
		new epub(options).promise.then(() => console.log('Done'));
	}
}