import { Chapter } from './Chapter.js';
import epub from 'epub-gen';
import { types } from './Website.js';


export class Novel {
	url: string;
	ePubName: string;
	title =  '';
	content: Chapter[] = [];
	altTitle = '';
	author = '';
	chapterCount = 0;
	synopsis = '';
	genres: string[] = [];
	//translators = '';
	//editors = '';
	contributors = '';
	tags = '';
	thumbnail: string|undefined = '';
	rating = 0;
	isCompleted = false;

   
	constructor(url: string, ePubName:string) {
		this.url = url;
		this.ePubName = ePubName;
	}

	toEPubConfig(){
		const options = {
			title: this.title,
			author: this.author,
			publisher: types.chrysanthemumgarden,
			cover: this.thumbnail,
			output: './data/'+this.ePubName+'.epub',
			content: [
				{
					title: 'Synopsis',
					data: this.synopsis,
					excludeFromToc: false,
					beforeToc: false
				},
				...this.content.map(ch => ch.toEPubConfig())
			],
			customOpfTemplatePath: './src/epub.opf.ejs'
		};
		new epub(options).promise.then(() => console.log('Done'));
	}
}