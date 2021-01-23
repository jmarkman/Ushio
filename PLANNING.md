# UshioBot

## Core Concept

UshioBot is my personal bot that I'll add features to as I see fit.

"lol nice anime bot IDIOT"

Ushio the real-life ship was a destroyer that performed a lot of "helper" functions such as towing, escort and defense, supply and troop transport, and rescue operations. Ushio the Kantai Collection shipgirl is modeled after this.

The shipgirls can also be modified with various parts to alter their stats, much like adding and removing bot components. There's also lots of fanart so the bot can cycle through avatars.

## Current Features I Have

- ~~"Regionalify" command~~ Removed as the functionality has been broken, regional indicator characters can now properly form flags in Discord messages
- Pokedex (need to make some various search changes to handle alolan pkmn and add rate limiting)

## Features I'd Like

- ~~Clip Service (get either Twitch clip or server-hosted clip and send as link to Discord, allowing Discord to embed it)~~ Added!
- Street Fighter 5 Frame Data (Yaksha bot has crappy move name inference and poor data organization)
- Steam API interaction to let users make !acct-trivia commands or similar to get minor trivia about their account
- RNG Command Family
	- Clone of ub3r-b0t's ".choose" command which will randomly select an item from the user's provided sequence of items
	- Clone of ub3r-b0t's ".roll D[n]" command for rolling n-sided dice for D&D-like things
	- Personal implementation of 8ball

## Handling Tables For Features

A legible way of separating things seems to be dividing features into global and local. Global features are those that are generic across all servers, like read-only queries for things like fighting game frame data. Local features vary per server, which include things like stored links and quotes. Global features will leverage an associated table (or tables) for a given global feature, and local features will generate a new table (or set of tables) for a given local feature.