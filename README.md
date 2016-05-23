# Inga
Micro backup solution designed to be running as a scheduled task daily.

# Configuration
Configuration basically is a list of tasks excuted sequentially. Everything to be configured in *App.config*.

Example:

https://github.com/goto10hq/Inga/blob/master/Inga/Inga/App.config
 
# Tasks

Global parameters:  
 
_type_ - type of task  
_on_ - on which days to run 

## AzureCleanUp

Clean up old files from Azure storage.  
 
_in_ - container name;directory  
_retention_ - number of versions of one file to be kept 

## CompressDirectories

Compress all directories in a given directory as separated archives. Each directory is packed recursively of course.  
 
_in_ - input directory  
_out_ - directory for storing archives 
*skip (optional)* - skip given directory(ies), comma separated list

## CompressDirectory

Compress one directory recursively.  
 
_in_ - input directory  
_out_ - directory for storing archives

## CopyFilesToAzure

Copy all files in a given directory to Azure storage.  
 
_in_ - input directory  
_out_ - container;"directory" (Azure storage doesn't have directories of course - so take it like a prefix path for all block blobs)
_connection_ - name of the connection string 

## LocalCleanUp

Clean up old files from local.  
 
_in_ - input directory  
_retention_ - number of versions of one file to be kept

## Run

Run executable.  

_in_ - executable file  
_out_ - optional arguments

## DocumentDb

Backup documentdb documents. At the moment all documents are fetched and saved in one json file.  
Filename is generated as database_collection_timestamp.json.

_out_ - output directory
_connection_ - name of the connection string 

## Todo

- possibility to force deletion even if retention says otherwise (ie. keep 1 but that archive is 6502 days old... so delete it anyway)
- filtering for DocumentDb (classic sql where)
