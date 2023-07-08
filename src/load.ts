import axios from 'axios';

export async function loadFromURL(url:string) {
	const response = await axios.get(url);
	return response.data;
}