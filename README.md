# promise
Let's play some rounds Promise Card Game against nine different ai opponents

## Usage:
in windows, open preferred console (cmd / powershell) and start
promise.exe

### startup parameters
#### botmatch
fast way to create 5 player 10-1-10 AI game

#### hidecards
dont render cards, faster botmatch

#### randombots
uses randomized bot AI's (maybe buggy)

#### usedb
records played games into mongodb
(totaltest branch also generates AI's by previously played games)

#### totaltest
generates 9000 times 20 games with random and evolutioned five player 10-1-10 games and saves results to mongodb

#### debugpromise
_from version 1.1_ shows ai's cards and some calculations when generating promise

## changelog
### v1.1 "nuutti"
* animated cards
* new ai players with simplified ai aiming to the better promises
* more random playing style to ai
* new launch parameter _debugpromise_ to help analyzing ai promise

### v1.02 "kinnunen"
first released version, includes quite buggy promise ai and played cards with very simple rules

## current ai opponents
* Jaska
* Pera
* Lissu
* Repa
* Arska
* Jossu
* Sussu
* Kake
* Late
and of course, the one and only, master of promises, **Fuison**
