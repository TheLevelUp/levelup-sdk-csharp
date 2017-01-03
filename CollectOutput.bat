@echo off
@echo.
@echo ---------------------------------------------------
@echo Collecting output from build under .\BuildOutput...
@echo ---------------------------------------------------
@echo.

set DEST=.\BuildOutput

set BINOBJ=bin
set REL=Release
set DBG=Debug

@rem Robocopy info: http://technet.microsoft.com/en-us/library/cc733145(v=WS.10).aspx
@rem Robocopy return codes: http://blogs.technet.com/b/deploymentguys/archive/2008/06/16/robocopy-exit-codes.aspx
@rem Robocopy options in use:
@rem /xo = exclude older files
@rem /xx = exclude extra files
@rem /xf = exclude file(s)
@rem /R:# = number of retry attempts
@rem /W:# = time to wait between retry attempts
@rem /NJH = No Job Header output (reduce noisy output)
@rem /NJS = No Job Summary output (reduce noisy output)

set SOURCE=.\LevelUp.Api.Client\%BINOBJ%
robocopy "%SOURCE%\%REL% 3.0" "%DEST%\%REL% 3.0" *.config *.dll *.pdb *.xml /xf *vshost* /xo /xx /R:5 /W:10 /NJH /NJS
robocopy "%SOURCE%\%REL% 3.5" "%DEST%\%REL% 3.5" *.config *.dll *.pdb *.xml /xf *vshost* /xo /xx /R:5 /W:10 /NJH /NJS
robocopy "%SOURCE%\%REL% 4.0" "%DEST%\%REL% 4.0" *.config *.dll *.pdb *.xml /xf *vshost* /xo /xx /R:5 /W:10 /NJH /NJS

robocopy "%SOURCE%\%DBG% 3.0" "%DEST%\%DBG% 3.0" *.config *.dll *.pdb *.xml /xf *vshost* /xo /xx /R:5 /W:10 /NJH /NJS
robocopy "%SOURCE%\%DBG% 3.5" "%DEST%\%DBG% 3.5" *.config *.dll *.pdb *.xml /xf *vshost* /xo /xx /R:5 /W:10 /NJH /NJS
robocopy "%SOURCE%\%DBG% 4.0" "%DEST%\%DBG% 4.0" *.config *.dll *.pdb *.xml /xf *vshost* /xo /xx /R:5 /W:10 /NJH /NJS

set SOURCE=.\LevelUp.Api.Utilities\%BINOBJ%
robocopy "%SOURCE%\%REL% 3.0" "%DEST%\%REL% 3.0" *.config *.dll *.pdb *.xml /xf *vshost* /xo /xx /R:5 /W:10 /NJH /NJS
robocopy "%SOURCE%\%REL% 3.5" "%DEST%\%REL% 3.5" *.config *.dll *.pdb *.xml /xf *vshost* /xo /xx /R:5 /W:10 /NJH /NJS
robocopy "%SOURCE%\%REL% 4.0" "%DEST%\%REL% 4.0" *.config *.dll *.pdb *.xml /xf *vshost* /xo /xx /R:5 /W:10 /NJH /NJS

robocopy "%SOURCE%\%DBG% 3.0" "%DEST%\%DBG% 3.0" *.config *.dll *.pdb *.xml /xf *vshost* /xo /xx /R:5 /W:10 /NJH /NJS
robocopy "%SOURCE%\%DBG% 3.5" "%DEST%\%DBG% 3.5" *.config *.dll *.pdb *.xml /xf *vshost* /xo /xx /R:5 /W:10 /NJH /NJS
robocopy "%SOURCE%\%DBG% 4.0" "%DEST%\%DBG% 4.0" *.config *.dll *.pdb *.xml /xf *vshost* /xo /xx /R:5 /W:10 /NJH /NJS
