**Update**: Helixbase now ships with HPP, so you get all the build performance benefits listed below just by using the [latest version of Helixbase](https://github.com/muso31/Helixbase). This page repository will remain for comparison's sake.

## Benchmarks

Below are the average build times from Helixbase's (HB) legacy Gulp-based scripts vs building from within Visual Studio using Helix Publishing Pipeline (HPP) with auto-publishing enabled.

| Mode | Clean (secs) | Subsequent no changes (secs) | Feature view/content change (secs) | Feature code change (secs) |
| --- | --- | --- | --- | --- |
|HB+Gulp "Publish-All-Projects"|135|49|57|66|
|HB+Gulp "Publish-Feature-Layer"|N/A|12|14|16|
|HB+HPP "CTRL+SHIFT+B"|60|4<sup title="No AppPool recycle"> ⚡</sup>|5<sup title="No AppPool recycle"> ⚡</sup>|9|

<sup>⚡</sup> No AppPool recycle

NOTE: The timing methods used were very non scientific, but the differences make any sub-second errors irrelevant anyway.

There are a number of reasons for the large discrepency:

* HB's "Publish-All-Projects" publishes individual projects after building the solution, effectively building all of the projects twice
* HB publishes each individual project, whereas HPP only invokes the publishing pipeline once
* Building within Visual Studio enables parallelism by default
* Visual Studio keeps information about previous builds in memory, so subsequent builds are more efficient
