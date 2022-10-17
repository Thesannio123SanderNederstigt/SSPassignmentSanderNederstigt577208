@description('Location of all resources')
param location string = resourceGroup().location

param storageAccounts_inh5946wrkshpdazwesa1_name string = 'inh5946wrkshpdazwesa1'
param serverfarms_INH_5946_WRKSHP_D_AZWE_ASP_1_name string = 'INH-5946-WRKSHP-D-AZWE-ASP-1'
param sites_assignmentsandernederstigt577208_name string = 'assignmentsandernederstigt577208'

// Storage Account
resource storageAccounts_inh5946wrkshpdazwesa1_name_resource 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  name: storageAccounts_inh5946wrkshpdazwesa1_name
  location: location
  tags: {
  }
  properties: {
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: false
    networkAcls: {
      bypass: 'AzureServices'
      virtualNetworkRules: []
      ipRules: []
      defaultAction: 'Allow'
    }
    supportsHttpsTrafficOnly: true
    encryption: {
      services: {
        file: {
          keyType: 'Account'
          enabled: true
        }
        blob: {
          keyType: 'Account'
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
    accessTier: 'Hot'
  }
}

// App Service Plan
resource serverfarms_INH_5946_WRKSHP_D_AZWE_ASP_1_name_default 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: serverfarms_INH_5946_WRKSHP_D_AZWE_ASP_1_name
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
    size: 'Y1'
    family: 'Y'
    capacity: 0
  }
  kind: 'functionapp'
  properties: {
    perSiteScaling: false
    elasticScaleEnabled: false
    maximumElasticWorkerCount: 1
    isSpot: false
    reserved: false
    isXenon: false
    hyperV: false
    targetWorkerCount: 0
    targetWorkerSizeId: 0
    zoneRedundant: false
  }
}

// Api Management Service
resource api_sites_assignmentsandernederstigt577208_resource 'Microsoft.ApiManagement/service@2021-12-01-preview' = {
  name: 'Functionsapi'
  tags: {
  }
  location: location
  properties: {
    publisherEmail: 'johndoe@email.com'
    publisherName: 'Doe, John'
    notificationSenderEmail: 'apimgmt-noreply@mail.windowsazure.com'
    hostnameConfigurations: [
      {
        type: 'Proxy'
        hostName: 'functionsapi.azure-api.net'
        negotiateClientCertificate: false
        defaultSslBinding: true
        certificateSource: 'BuiltIn'
      }
    ]
    customProperties: {
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Protocols.Tls10': 'False'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Protocols.Tls11': 'False'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Tls10': 'False'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Tls11': 'False'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Security.Backend.Protocols.Ssl30': 'False'
      'Microsoft.WindowsAzure.ApiManagement.Gateway.Protocols.Server.Http2': 'False'
    }
    virtualNetworkType: 'None'
    apiVersionConstraint: {
    }
    publicNetworkAccess: 'Enabled'
  }
  sku: {
    name: 'Consumption'
    capacity: 0
  }
}

// Storage Account Blob Service
resource storageAccounts_inh5946wrkshpdazwesa1_name_blobStorage 'Microsoft.Storage/storageAccounts/blobServices@2022-05-01' = {
  parent: storageAccounts_inh5946wrkshpdazwesa1_name_resource
  name: 'default'
  properties: {
    changeFeed: {
      enabled: false
    }
    restorePolicy: {
      enabled: false
    }
    containerDeleteRetentionPolicy: {
      enabled: true
      days: 7
    }
    cors: {
      corsRules: []
    }
    deleteRetentionPolicy: {
      allowPermanentDelete: false
      enabled: true
      days: 7
    }
    isVersioningEnabled: false
  }
}

// Storage Account File Service
resource storageAccounts_inh5946wrkshpdazwesa1_name_fileStorage 'Microsoft.Storage/storageAccounts/fileServices@2022-05-01' = {
  parent: storageAccounts_inh5946wrkshpdazwesa1_name_resource
  name: 'default'
  properties: {
    shareDeleteRetentionPolicy: {
      enabled: true
      days: 7
    }
  }
}

// Storage Account Queue Service
resource storageAccounts_inh5946wrkshpdazwesa1_name_queueStorage 'Microsoft.Storage/storageAccounts/queueServices@2022-05-01' = {
  parent: storageAccounts_inh5946wrkshpdazwesa1_name_resource
  name: 'default'
  properties: {
    cors: {
      corsRules: []
    }
  }
}

// Storage Account Table Service
resource storageAccounts_inh5946wrkshpdazwesa1_name_tableStorage 'Microsoft.Storage/storageAccounts/tableServices@2022-05-01' = {
  parent: storageAccounts_inh5946wrkshpdazwesa1_name_resource
  name: 'default'
  properties: {
    cors: {
      corsRules: []
    }
  }
}

// Azure Function App
resource sites_assignmentsandernederstigt577208_name_resource 'Microsoft.Web/sites@2022-03-01' = {
  name: sites_assignmentsandernederstigt577208_name
  kind: 'functionapp'
  location: location
  properties: {
    enabled: true
    hostNameSslStates: [
      {
        name: 'assignmentsandernederstigt577208.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: 'assignmentsandernederstigt577208.scm.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Repository'
      }
    ]
    serverFarmId: serverfarms_INH_5946_WRKSHP_D_AZWE_ASP_1_name_default.id
    reserved: false
    isXenon: false
    hyperV: false
    vnetRouteAllEnabled: false
    vnetImagePullEnabled: false
    vnetContentShareEnabled: false
    siteConfig: {
      numberOfWorkers: 1
      linuxFxVersion: ''
      acrUseManagedIdentityCreds: false
      alwaysOn: false
      http20Enabled: false
      functionAppScaleLimit: 200
      minimumElasticInstanceCount: 0
    }
    scmSiteAlsoStopped: false
    clientAffinityEnabled: false
    clientCertEnabled: false
    clientCertMode: 'Required'
    hostNamesDisabled: false
    customDomainVerificationId: '048DBECC53929DB491656B86DA5A7B082707D9686BA87CEA51837FEFA289B00D'
    containerSize: 1536
    dailyMemoryTimeQuota: 0
    httpsOnly: true
    redundancyMode: 'None'
    storageAccountRequired: false
    keyVaultReferenceIdentity: 'SystemAssigned'
  }
}

// Function App ftp
resource sites_assignmentsandernederstigt577208_name_ftp 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2022-03-01' = {
  parent: sites_assignmentsandernederstigt577208_name_resource
  name: 'ftp'
  location: location
  properties: {
    allow: true
  }
}

// Function App scm
resource sites_assignmentsandernederstigt577208_name_scm 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2022-03-01' = {
  parent: sites_assignmentsandernederstigt577208_name_resource
  name: 'scm'
  location: location
  properties: {
    allow: true
  }
}

// Function App Configuration
resource sites_assignmentsandernederstigt577208_name_web 'Microsoft.Web/sites/config@2022-03-01' = {
  parent: sites_assignmentsandernederstigt577208_name_resource
  name: 'web'
  location: location
  properties: {
    numberOfWorkers: 1
    defaultDocuments: [
      'Default.htm'
      'Default.html'
      'Default.asp'
      'index.htm'
      'index.html'
      'iisstart.htm'
      'default.aspx'
      'index.php'
    ]
    netFrameworkVersion: 'v6.0'
    phpVersion: ''
    pythonVersion: ''
    nodeVersion: ''
    powerShellVersion: ''
    linuxFxVersion: ''
    requestTracingEnabled: false
    remoteDebuggingEnabled: false
    httpLoggingEnabled: false
    acrUseManagedIdentityCreds: false
    logsDirectorySizeLimit: 35
    detailedErrorLoggingEnabled: false
    publishingUsername: '$assignmentsandernederstigt577208'
    scmType: 'None'
    use32BitWorkerProcess: true
    webSocketsEnabled: false
    alwaysOn: false
    appCommandLine: ''
    managedPipelineMode: 'Integrated'
    virtualApplications: [
      {
        virtualPath: '/'
        physicalPath: 'site\\wwwroot'
        preloadEnabled: false
        virtualDirectories: null
      }
    ]
    loadBalancing: 'LeastRequests'
    experiments: {
      rampUpRules: []
    }
    autoHealEnabled: false
    vnetName: ''
    vnetRouteAllEnabled: false
    vnetPrivatePortsCount: 0
    cors: {
      allowedOrigins: [
        'https://portal.azure.com'
      ]
      supportCredentials: false
    }
    apiDefinition: {
      url: '/api/swagger.json'
    }
    localMySqlEnabled: false
    ipSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 2147483647
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 2147483647
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictionsUseMain: false
    http20Enabled: false
    minTlsVersion: '1.2'
    scmMinTlsVersion: '1.2'
    ftpsState: 'FtpsOnly'
    preWarmedInstanceCount: 0
    functionAppScaleLimit: 0
    functionsRuntimeScaleMonitoringEnabled: false
    minimumElasticInstanceCount: 0
    azureStorageAccounts: {
    }
  }
}

// Function App HostName Bindings
resource sites_assignmentsandernederstigt577208_name_site_bindings 'Microsoft.Web/sites/hostNameBindings@2022-03-01' = {
  parent: sites_assignmentsandernederstigt577208_name_resource
  name: '${sites_assignmentsandernederstigt577208_name}.azurewebsites.net'
  location: location
  properties: {
    siteName: 'assignmentsandernederstigt577208'
    hostNameType: 'Verified'
  }
}

// Storage Account WebJobs Hosts
resource storageAccounts_inh5946wrkshpdazwesa1_name_blobStorage_azure_webjobs_hosts 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-05-01' = {
  parent: storageAccounts_inh5946wrkshpdazwesa1_name_blobStorage
  name: 'azure-webjobs-hosts'
  properties: {
    immutableStorageWithVersioning: {
      enabled: false
    }
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
}

// Storage Account WebJobs Secrets
resource storageAccounts_inh5946wrkshpdazwesa1_name_blobStorage_webjobs_secrets 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-05-01' = {
  parent: storageAccounts_inh5946wrkshpdazwesa1_name_blobStorage
  name: 'azure-webjobs-secrets'
  properties: {
    immutableStorageWithVersioning: {
      enabled: false
    }
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
}

// Storage Account Blob Storage Container
resource storageAccounts_inh5946wrkshpdazwesa1_name_blobstorage_imagescontainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-05-01' = {
  parent: storageAccounts_inh5946wrkshpdazwesa1_name_blobStorage
  name: 'imagescontainer'
  properties: {
    immutableStorageWithVersioning: {
      enabled: false
    }
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
}

// Storage Account File Service Shares
resource storageAccounts_inh5946wrkshpdazwefa1_name_fileshare_inh5946wrkshpdazwefa1 'Microsoft.Storage/storageAccounts/fileServices/shares@2022-05-01' = {
  parent: storageAccounts_inh5946wrkshpdazwesa1_name_fileStorage
  name: 'inh5946wrkshpdazwefa1'
  properties: {
    accessTier: 'TransactionOptimized'
    shareQuota: 5120
    enabledProtocols: 'SMB'
  }
}

// Storage Account Queue Storage
resource storageAccounts_inh5946wrkshpdazwesa1_name_queueStorage_imagesqueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2022-05-01' = {
  parent: storageAccounts_inh5946wrkshpdazwesa1_name_queueStorage
  name: 'imagesqueue'
  properties: {
  }
}

// Insights
resource serverfarms_INH_5946_WRKSHP_D_AZWE_ASP_1_name_resource 'Microsoft.Insights/components@2020-02-02' = {
  name: serverfarms_INH_5946_WRKSHP_D_AZWE_ASP_1_name
  location: location
  tags: {
  }
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Flow_Type: 'Bluefield'
    Request_Source: 'rest'
    RetentionInDays: 30
    IngestionMode: 'ApplicationInsights'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}
