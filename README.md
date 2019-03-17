# xivtool

Parses game data and does things with that data with semi-pluggable modules. Deals with localised strings by providing an interface to all localised sheets at the same time. MULTI LANGUAGE DRIFTING

Partially based upon an older project to port all the data to a SQL db

## Usage

    xivtool <data path> <module name> [module [args [go [here]]]]
    
Modules should complain if required arguments are missing.
    
## Notes

* Modules are located in the `Module` namespace and only modules inside that namespace can be used.
