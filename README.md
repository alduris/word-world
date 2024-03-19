# Word World
A Rain World mod that turns things into words. Lots of things. Download from the releases in the sidebar.

## Bugs and nasty things
This mod will automatically shut off _all_ of its effects in the event something goes wrong. If everything starts showing up again like normal, you will know this happened. If you wish to report any issues, send a report in the issues section of this GitHub (account required) and upload the `LogOutput.log` (located in {Rain World installation folder}/BepInEx from the playthrough this happened (do not restart the game in between).

## Things covered
You want the full list? Well, unless I forgot some or haven't updated the list in the future:
- Artificer's drone (MSC)
- Batflies
- Batnip
- Beehives
  - Bees included
- Big spiders
- Bubble fruit
- Bubble weed
- Centipedes (all, including custom ones)
- Cherrybombs
- Coalescipedes
- Cosmetic insects (all, including custom ones)
- Dandelion peaches (MSC)
- Dropwigs
- Echoes
- Eggbugs and firebugs (MSC)
  - Their eggs included
- Flashbangs/flare bombs
- Garbage worms
- Glow weed (MSC)
- Gold flakes
- Gooieducks (MSC)
- Grapple worms
- Green sparks
- Guardians
- Hanging fruit/blue fruit/whatever you call it
- Hazers
- Inspectors (MSC)
  - And their eyes
- Iterators (all)
  - That includes custom ones, though their names are not supported by default
- Jellyfish
  - Plus big jellyfish (MSC) and their death jelly
- Jetfish
- Joke rifle (technically MSC)
  - Plus the bullets it makes
- Karma flowers
- Lanterns
- Lantern mice
- Leeches
- Leviathans
- Lillypucks
- Lizards (all, including custom ones)
  - Including their spit
- Miros birds
- Monster kelp
- Moon's cloak (MSC)
- Mushrooms
- Neuron flies (all)
- Noodleflies (all)
  - Their eggs included
- Overseers
  - And their eyes
- Pearls
- Popcorn plants
- Rain deer
- Rarefaction cell (MSC)
- Rocks
- Rot cysts (all: dlls, blls, mll/tlls (MSC), and custom ones)
- Scavengers (all)
- Scavenger bombs
- Seeds (MSC)
- Singularity bombs (MSC)
- Sky dandelions
- Slime mold
- Slugcats and slugpups (MSC)
- Snails
- Spears
- Spitter spiders
  - Including their dart maggots
- Spore puffs
- Stowaways (MSC)
- Void spawn
- Vultures (all)
- Vulture grubs
- Vulture masks
- Worm grass
- Yeeks (MSC)

This is just the list of things that are fully supported, more things are to come. Most if not all custom creatures should have basic support too.

## API
Why does this have an API? ¯\\\_(ツ)\_/¯

You can add your own wordifications or even replace my implementations. You can also register custom iterator names using it. Reference the mod's binary in your code project and then use the methods below. The `WordWorld.WordUtil` class also contains helpful methods not listed here that you can use in making your custom implementations (importantly including the font size constant and a method for calculating text width, used in dynamically scaling your text properly).

### RegisterItem
To add an implementation, call `WordWorld.WordAPI.RegisterItem`.

- **Arguments**:
  - `Type` type: the type of `IDrawable` you intend to wordify. Use `typeof` to get the type of an object.
  - `Func<IDrawable, RoomCamera.SpriteLeaser, FLabel[]>` initLabelsFunc: an initialization function for your labels. This is called once per object after the first call to `IDrawable.DrawSprites`.
    - This method should be used to initialize your `FLabel`s and perform any styling that will not change at any point.
    - _Arguments_:
      - `IDrawable` obj: an object of the type passed in previously
      - `RoomCamera.SpriteLeaser` sLeaser: the sprite leaser
    - _Returns_:
      - `FLabel[]`: an array of the labels the mod uses
  - `Action<IDrawable, FLabel[], RoomCamera.SpriteLeaser, float, Vector2>` drawLabelsFunc: a draw function for your labels. This is called after every call to `IDrawable.DrawSprites` and after initLabelsFunc.
    - _Arguments_:
      - `IDrawable` obj: an object of the type passed in previously
      - `FLabel[]` labels: the labels you created for it
      - `RoomCamera.SpriteLeaser` sLeaser: the sprite leaser
      - `float` timeStacker: the time stacker (use it to lerp between `lastPos` and `pos` where applicable)
      - `Vector2` camPos: the camera position, you should subtract this from any positions not directly taken from any sprites
    - _Returns_:
      - Nothing
- **Returns**:
  - Nothing

### UnregisterItem
Any custom implementations can be unregistered by calling `WordWorld.WordAPI.UnregisterItem` I'm not sure why you'd need to do this but it's there if you need it.

- **Arguments**:
  - `Type` type: the type of `IDrawable` you intend to unregister.
- **Returns**:
  - `bool`: whether or not the item was actually unregistered.

### RegisterIteratorName
To register an iterator name, call `WordWorld.WordAPI.RegisterIteratorName`.

- **Arguments**:
  - `Oracle.OracleID` id: the iterator's ID you want to register
  - `string` name: the name of the iterator
- **Returns**:
  - Nothing

### UnregisterIteratorName
If you want to unregister a custom iterator name, call `WordWorld.WordAPI.UnregisterIteratorName`. Like `UnregisterItem`, I'm not sure why you'd want to do this, but the option is there if you want it. Keep in mind the base game iterator's names are hardcoded, so unless you overrode those ones, this will not work for those.

- **Arguments**:
  - `Oracle.OracleID` id: the iterator's ID you want to register
  - `string` name: the name of the iterator
- **Returns**:
    - `bool`: whether or not the name was actually unregistered.
