
export class Chapter {
	title =  '';
	number = 0;
	content: string[] = [];
   
	constructor(title: string) {
		this.title = title;
	}

	toEPubConfig(){
		return {
			title: this.title,
			data: this.content.join('')
		};
	}
}