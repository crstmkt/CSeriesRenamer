# CSeriesRenamer

This program has no input parameters or something else. It works on a hard-codec pattern and transforms this to another hard-coded pattern to make files recognizable by KODI.

The Program has to be executed in the root directory of your Media-Archive. Hard-coded pattern is

< root > \ < Name of series > \ Staffel < Seasonnumer in Format 00 > \ < Episodenumber in Format 00 > < EpisodeTitle >
  
  Example:
  
  < root > \ Game of Thrones\Staffel 01\01 Der Winter naht.ext
  
  Transforming this to:
  
  < root > \ Game of Thrones\Staffel 01\ Game of Thrones - S01E01 - Der Winter naht.ext
  
  The program executes .avi, .mkv, .mov and .mp4 files.
