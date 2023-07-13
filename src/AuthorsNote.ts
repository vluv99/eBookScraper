export class AuthorsNote {
	id: string;
	note = '';
	content = '';
	constructor(id: string, content: string) {
		this.id = id;
		this.content = content;
	}
}