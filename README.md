# Loyalty Services

Repo containing the services required for Loyalty
- Dynamo & S3 interfaces to retrieving/storing data
- GraphQL server to interact with the interfaces

## GraphQL Endpoint

Data is retrieved through a dotnet (Hot Chocolate) GraphQL server found [here](http://loyalty-api-loadbalancer-455480432.ap-southeast-2.elb.amazonaws.com/graphql/)

### Data types

```
account {
  birthday: DateTime!
  email: String
  firstName: String
  lastName: String
  loyalties: [String]
  username: String
}
```

```
organization {
  username: String
  name: String
  description: String
  type: OrganizationType!
  address: String
  color: String

  media: OrganizationMedia {
    cover: String
    images: [String]
    logo: String
  }

  employees: [Employee] {
    ...
  }
}
```

```
employee {
  username: String
  organizationUsername: String
  firstName: String
  lastName: String
  dateStarted: DateTime!
  description: String
  position: String

  media: EmployeeMedia {
    images: [String]
    profileImage: String
  }
}
```

### Queries

```
getAccount(accountUsername: "ACC_USERNAME") {
  birthday
  email
  firstName
  lastName
  loyalties
  username
}
```

```
getAllOrganizations {
  username
  name
  description
  type
  address
  color
  
  media {
    cover
    images
    logo
  }
  
  employees {
    username
    organizationUsername
    firstName
    lastName
    dateStarted
    description
    position
    
    media {
      images
      profileImage
    }
  }
}
```

```
getOrganization(organizationUsername: "ORG_USERNAME") {
  username
  name
  description
  type
  address
  color
  
  media {
    cover
    images
    logo
  }
  
  employees {
    username
    organizationUsername
    firstName
    lastName
    dateStarted
    description
    position
    
    media {
      images
      profileImage
    }
  }
}
```

```
getOrganizationsOfType(organizationType: .ORG_TYPE) {
  username
  name
  description
  type
  address
  color
  
  media {
    cover
    images
    logo
  }
  
  employees {
    username
    organizationUsername
    firstName
    lastName
    dateStarted
    description
    position
    
    media {
      images
      profileImage
    }
  }
}
```
