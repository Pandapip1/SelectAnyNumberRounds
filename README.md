# Pick Any Number: A Rounds Mod by Pandapip1

This is a mod for the game [ROUNDS](https://store.steampowered.com/app/1557740/ROUNDS/), made by [Pandapip1](https://github.com/Pandapip1). It overrides the default card selection screen, allowing you to pick any number of cards you want, instead of just one.

**This mod is in beta. There will be bugs. When you find them, please report them on the [issues page](https://github.com/Pandapip1/SelectAnyNumberRounds/issues).**

## Installation

Pick Any Number is available on [Thunderstore](https://rounds.thunderstore.io/package/Pandapip1/SelectAnyNumber/).

## Compatibility

| Mod                                                                                                    | Compatibility                       |
| ------------------------------------------------------------------------------------------------------ | ----------------------------------- |
| [Rounds With Friends](https://rounds.thunderstore.io/package/olavim/RoundsWithFriends/)                | ✅ Full (Dependency)                |
| [Pick N Cards](https://rounds.thunderstore.io/package/Pykess/Pick_N_Cards/)                            | ✅ Full (Dependency)                |
| [Lobby Improvements](https://rounds.thunderstore.io/package/RoundsModdingCommunity/LobbyImprovements/) | ✅ Full                             |
| [Cosmic Rounds](https://rounds.thunderstore.io/package/XAngelMoonX/CR/)                                | ✅ Full                             |
| [Root Cards](https://rounds.thunderstore.io/package/Root/Root_Cards/)                                  | ⚠️ Partial (Untested Fix) [^1]      |
| [Will's Wacky Gamemodes](https://rounds.thunderstore.io/package/willuwontu/WillsWackyGameModes/)       | ⚠️ Partial (Untested Fix) [^2]      |
| [Pick Timer](https://rounds.thunderstore.io/package/otDan/PickTimer/)                                  | 🛑 Incompatible (Untested Fix) [^3] |
| [Tab Info](https://rounds.thunderstore.io/package/willuwontu/TabInfo/)                                 | 🛑 Incompatible [^4]                |
| [Cards Plus](https://rounds.thunderstore.io/package/willis81808/CardsPlus/)                            | ❓ Untested                         |
| [Will's Wacky Cards](https://rounds.thunderstore.io/package/willuwontu/WillsWackyCards/)               | ❓ Untested                         |
| [Chaos Poppycars Cards](https://rounds.thunderstore.io/package/poppycars/ChaosPoppycarsCards/)         | ❓ Untested                         |
| [Gear Up Cards](https://rounds.thunderstore.io/package/GearUP/GearUpCards/)                            | ❓ Untested                         |
| [Boss Sloths's Cards](https://rounds.thunderstore.io/package/BossSloth/BSC/)                           | ❓ Untested                         |

If you have tested this mod with another mod and it is not listed here, please [update this table](https://github.com/Pandapip1/SelectAnyNumberRounds/edit/main/README.md) and submit a pull request!

## License

### Content Creator

If you are a content creator on YouTube or any other similar platform and would like to use this mod in your video, you may do so by conspicuously crediting me and linking to the Thunderstore page in the description of your video using the following text:

> This video uses the mod "Pick Any Number" by Pandapip1, which can be found at https://rounds.thunderstore.io/package/Pandapip1/SelectAnyNumber/

### Source Code

The source code for this mod is licensed under the MIT License. In summary, you are free to use this code in any way you want, as long as you include the license file that credits me. The full license can be found in the [LICENSE.md](LICENSE.md) file.

[^1]: Due to an unusual interaction, picking "Distill Acquisition" results in a softlock. Potentially fixed by [2cba542cfa00e7315f3c567e48a4c49c582689a7](https://github.com/Pandapip1/SelectAnyNumberRounds/commit/2cba542cfa00e7315f3c567e48a4c49c582689a7)

[^2]: Pick any number does not work with the draft gamemode. It will still work with the other gamemodes. Potentially fixed by [c09175831be3a26de30fd0f10a272c37d40f4df6](https://github.com/Pandapip1/SelectAnyNumberRounds/commit/c09175831be3a26de30fd0f10a272c37d40f4df6)

[^3]: When the timer runs out, you pick a random card, and the timer goes away. However, if it was not the continue card, you can continue picking cards. Potentially fixed by [86d879f4aa6de4de43633c0db9e8109bf63e1fa7](https://github.com/Pandapip1/SelectAnyNumberRounds/commit/86d879f4aa6de4de43633c0db9e8109bf63e1fa7)

[^4]: Reported by user. Tries to create and display endless "BU" cards once continue is picked: ![Provided screenshot](https://media.discordapp.net/attachments/1095772439172091935/1096163350527881226/image.png)
