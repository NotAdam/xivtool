# xivtool

Parses game data and does things with that data with semi-pluggable modules. Deals with localised strings by providing an interface to all localised sheets at the same time. MULTI LANGUAGE DRIFTING

Partially based upon an older project to port all the data to a SQL db

todo: fix this readme lol


## Usage

    xivtool <data path> <parser name> [module] [args] [here...]
    
## Notes

* Modules are located in the `Module` namespace and only modules inside that namespace can be used.
