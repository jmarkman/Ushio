# Ushio - A general purpose Discord bot for (mostly?) my own usage

![ushio!](https://i.imgur.com/qpFI6OW.png)

Ushio is my swiss-army-knife Discord bot that I'm building for mostly my own usage. Whenever I go, "damn I wish {some existing bot} could do that", I'll play around with that idea here.

This is also a playground for learning about bot developlment and deployment. I currently don't have anything at all in regards to deployment.

## Current Features

- Pokedex API

By typing `!pokedex [pokemon name]`, Ushio will hit the api at [PokeApi](https://pokeapi.co/) and get a little pokedex entry for the provided pokemon.

- Current Weather Forecast for a United States ZIP Code

By typing `!weather [5-digit ZIP code]`, Ushio will hit the api at [OpenWeatherMap](https://openweathermap.org/api) and get current forecast data for that region.

- Video clip "bookmarking"

Users in a server can bookmark video clips associated with in-jokes or whatever context and then recall them at a moment's notice with a search.

- Fighting game "vod" search

Using results from several fighting game "vod" channels (channels where users upload recorded matches of high-level players, "vod" comes from "Twitch vod"), users can search for vods of either specific players, characters, or both, and get a video for that search criteria back if one exists. Largest feature so far, currently a work-in-progress as it only supports searches for the fighting game Guilty Gear -Strive-.

- RNG commands

I've implemented knockoffs of `!choose [set of items delimited by commas]` and `!8ball`.