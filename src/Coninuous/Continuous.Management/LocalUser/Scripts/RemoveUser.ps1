﻿#  
# Remove local user by username
#
param([string]$name)
  
net user $name /delete