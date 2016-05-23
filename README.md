# Inga
Micro backup solution designed to be running as a scheduled task daily.

# Configuration
Configuration basically is a list of tasks excuted sequentially. Everything to be configured in *App.config*.

Example:

https://github.com/goto10hq/Inga/blob/master/Inga/Inga/App.config
 
# Tasks

Global parameters:  
 
**type** - type of task  
**on** - on which days to run 

## AzureCleanUp

Clean up old files from Azure storage.  
 
**in** - container name;directory  
**retention** - number of versions of one file to be kept 

## CompressDirectories

Compress all directories in a given directory as separated archives. Each directory is packed recursively of course.  
 
**in** - input directory  
**out** - directory for storing archives  
_skip (optional)_ - skip given directory(ies), comma separated list

## CompressDirectory

Compress one directory recursively.  
 
**in** - input directory  
**out** - directory for storing archives

## CopyFilesToAzure

Copy all files in a given directory to Azure storage.  
 
**in** - input directory  
**out** - container;"directory" (Azure storage doesn't have directories of course - so take it like a prefix path for all block blobs)  
**connection** - name of the connection string 

## LocalCleanUp

Clean up old files from local.  
 
**in** - input directory  
**retention** - number of versions of one file to be kept

## Run

Run executable.  

**in** - executable file  
**out** - optional arguments

## DocumentDb

Backup documentdb documents. At the moment all documents are fetched and saved in one json file.  
Filename is generated as database_collection_timestamp.json.

**out** - output directory  
**connection** - name of the connection string 

## Todo

- possibility to force deletion even if retention says otherwise (ie. keep 1 but that archive is 6502 days old... so delete it anyway)
- filtering for DocumentDb (classic sql where)
