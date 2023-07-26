import { Novel } from './Novel.js';
import { Website, types } from './Website.js';

export const sites = {
	test: new Website(types.test, 'https://example.com', []),
    
	chrysanthemumgarden: new Website(types.chrysanthemumgarden, 'https://chrysanthemumgarden.com/', [
		new Novel('https://chrysanthemumgarden.com/novel-tl/fog/', 'FOG'),
		new Novel('https://chrysanthemumgarden.com/novel-tl/iar/', 'iar')
	]),

	wuxiaworld: new Website(types.wuxiaworld, 'https://wuxiaworld.eu/', [
		new Novel('https://wuxiaworld.eu/novel/i-just-want-to-be-in-a-relationship', 'i-just-want-to-be-in-a-relationship'),
		//new Novel('https://wuxiaworld.eu/novel/superstar-from-age-0', 'superstar-from-age-0'),
		new Novel('https://wuxiaworld.eu/novel/the-novels-extra', 'the-novels-extra'),
		new Novel('https://wuxiaworld.eu/novel/semantic-error', 'semantic-error'),
		new Novel('https://wuxiaworld.eu/novel/heaven-officials-blessing', 'heaven-officials-blessing'),
		new Novel('https://wuxiaworld.eu/novel/omniscient-readers-viewpoint', 'omniscient-readers-viewpoint'),
		new Novel('https://wuxiaworld.eu/novel/ending-maker', 'ending-maker'),
		//new Novel('https://wuxiaworld.eu/search/global%20examination', 'global-examination'),
		new Novel('https://wuxiaworld.eu/novel/the-kings-avatar', 'the-kings-avatar'),
		new Novel('https://wuxiaworld.eu/novel/leveling-with-the-gods', 'leveling-with-the-gods'),
		new Novel('https://wuxiaworld.eu/novel/i-became-the-childhood-friend-of-the-obsessive-second-male-lead', 'i-became-the-childhood-friend-of-the-obsessive-second-male-lead'),
		new Novel('https://wuxiaworld.eu/novel/i-played-the-role-of-the-adopted-daughter-too-well', 'i-played-the-role-of-the-adopted-daughter-too-well'),
		new Novel('https://wuxiaworld.eu/novel/max-talent-player', 'max-talent-player'),
		new Novel('https://wuxiaworld.eu/novel/the-husky-and-his-white-cat-shizun', 'the-husky-and-his-white-cat-shizun'),
		new Novel('https://wuxiaworld.eu/novel/sick-beauty-rebirth', 'sick-beauty-rebirth'),
		new Novel('https://wuxiaworld.eu/novel/i-quit-being-the-male-leads-rival', 'i-quit-being-the-male-leads-rival'),
		new Novel('https://wuxiaworld.eu/novel/dungeon-maker', 'sick-beauty-rebirth'),
		new Novel('https://wuxiaworld.eu/novel/glory', 'glory'),
		new Novel('https://wuxiaworld.eu/novel/accidental-mark', 'accidental-mark'),
		new Novel('https://wuxiaworld.eu/novel/surprise-the-supposed-talent-show-was-actually', 'surprise-the-supposed-talent-show-was-actually'),
		new Novel('https://wuxiaworld.eu/novel/you-use-a-gun-i-use-a-bow', 'you-use-a-gun-i-use-a-bow'),
		new Novel('https://wuxiaworld.eu/novel/awm-pubg', 'awm-pubg'),
		new Novel('https://wuxiaworld.eu/novel/dont-you-like-me', 'dont-you-like-me'),
		new Novel('https://wuxiaworld.eu/novel/god-level-summoner', 'god-level-summoner'),
		new Novel('https://wuxiaworld.eu/novel/dont-pick-up-boyfriends-from-the-trash-bin', 'dont-pick-up-boyfriends-from-the-trash-bin'),
		new Novel('https://wuxiaworld.eu/novel/true-star', 'true-star'),
		new Novel('https://wuxiaworld.eu/novel/superstar-aspirations', 'superstar-aspirations'),
		new Novel('https://wuxiaworld.eu/novel/rebirth-of-a-movie-star', 'rebirth-of-a-movie-star'),
	])
};