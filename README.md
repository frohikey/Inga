# Inga
Micro backup solution designed to be running as a scheduled task daily.

# Configuration
Configuration basically is a list of tasks excuted sequentially. Everything to be configured in *App.config*.

Example:

```xml
<inga logType="SimpleLogFile" logFile="d:\inga\inga.log">    
    <tasks>
      <task name="Pack inetpub" type="CompressDirectories" in="c:\wwwroot\inetpub" out="d:\inga\inetpub" on="Friday,Thursday,Wednesday"/>
      <task name="Pack logs" type="CompressDirectory" in="c:\logs" out="d:\inga\logs" />
      <task name="Run magic script" type="Run" in="d:\inga\run\magic.bat" out="-a 1 -b 2 -c 3"/>
      <task name="Cleanup local inetpub" type="LocalCleanUp" in="c:\wwwroot\inetpub" retention="3"/>      
      <task name="Copy inetpub to Azure" type="CopyFilesToAzure" in="d:\inga\inetpub" out="inga;server1/inetpub" />
      <task name="Cleanup azure inetpub" type="AzureCleanUp" in="inga;server1/inetpub" retention="3"/>
    </tasks>
  </inga>
```
 
# Tasks

Global parameters:

*type* - type of task
*on* - on which days to run

## AzureCleanUp

Clean up old files from Azure storage.

*in* - container name;directory
*retention* - number of versions of one file to be kept

## CompressDirectories

Compress all directories in a given directory as separated archives. Each directory is packed recursively of course.

*in* - input directory
*out* - directory for storing archives

## CompressDirectory

Compress one directory recursively.

*in* - input directory
*out* - directory for storing archives

## CopyFilesToAzureTask

Copy all files in a given directory to Azure storage.

*in* - input directory
*out* - container;"directory" (Azure storage doesn't have directories of course - so take it like a prefix path for all block blobs)

## LocalCleanUp

Clean up old files from local.

*in* - input directory
*retention* - number of versions of one file to be kept

## RunTask

Run executable.

*in* - executable file
*out* - optional arguments

