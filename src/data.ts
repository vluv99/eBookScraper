import { Novel } from './Novel.js';
import { Website, types } from './Website.js';

export const sites = {
	test: new Website(types.test, 'https://example.com', []),
    
	chrysanthemumgarden: new Website(types.chrysanthemumgarden, 'https://chrysanthemumgarden.com/', [
		new Novel('https://chrysanthemumgarden.com/novel-tl/fog/', 'FOG'),
		new Novel('https://chrysanthemumgarden.com/novel-tl/iar/', 'iar')
	]),

	wuxiaworld: new Website(types.wuxiaworld, 'https://wuxiaworld.eu/', [
		new Novel('https://wuxiaworld.eu/novel/i-just-want-to-be-in-a-relationship', 'i-just-want-to-be-in-a-relationship')
	])
};