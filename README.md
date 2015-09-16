# KeePassHttpCli

KeePassHttpCli is a command line client for accessing a [KeePass](http://keepass.info/) database through [KeePassHttp](https://github.com/pfn/keepasshttp/).

I've searched for a KeePass command line client, found nothing - so I created this together with [KeePassHttpClient](https://github.com/berrnd/KeePassHttpClient).

Currently not every request type is implemented, see the usage examples below or the help screen.

## How to use
Mainly everything should be self explanatory through the help option, just start KeePassHttpCli.exe without any or with the `--help` parameter.
```
KeePassHttpCli 1.1.0.0
Copyright © Bernd Bestel 2015
https://github.com/berrnd/KeePassHttpCli

  -a, --action           Action, have to be one of the following strings (see
                         explanation below): associate, get-single-password

  -f, --search-field     Search field, have to be one the following strings:
                         url, any

  -s, --search-string    Search string

  -o, --stay-open        Keeps the console window open

  --help                 Display this help screen.


Actions:

associate: Associate a new KeePass database, connection info is stored encrypted (can only be decrypted by the current logged in user) in %localappdata%\KeePassHttpCli

get-single-password: Get a single password in plain text (StdOut), if more than one entry is received, the first one is taken
```

## Usage examples
Associate a KeePass database, association info will be stored (encrypted, readable only by the currently logged in user) in `%localappdata%\KeePassHttpCli`)

`KeePassHttpCli.exe -a associate`

Get a single password as the output (StdOut), from an entry with a matching URL `google.com`, and the keep the console window open

`KeePassHttpCli.exe -a "get-single-password" -f url -s "google.com" -o`

Get a single password, from the first entry that matches the generic search `google` (like through the quick search box in KeePass)

`KeePassHttpCli.exe -a "get-single-password" -f any -s "google"`
