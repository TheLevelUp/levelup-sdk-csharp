## Base Uris ##
$productionBaseURI = "https://api.thelevelup.com/"
$stagingBaseURI = "https://api.staging-levelup.com/"
$sandboxBaseURI = "https://sandbox.thelevelup.com/"

#####################
## LEVELUP API v14 ##
#####################

$v14 = "v14/"
$v15 = "v15/"

$global:ver = $v14
$global:baseURI = $productionBaseURI
$global:uri = $global:baseURI + $global:ver

$headers = @{"Content-Type" = "application/json"; "Accept" = "application/json"}

$environments = @{"production" = $productionBaseURI; "sandbox" = $sandboxBaseURI; "staging" = $stagingBaseURI }

## Manage Environment ##
function Get-LevelUpEnvironment() {
    Write-Host $global:baseURI
}

function Set-LevelUpEnvironment([string]$envName) {
    Set-LevelUpEnvironment $envName 14
}

function Set-LevelUpEnvironment([string]$envName, [int]$version) {
    $global:ver = $v14

    if(!$environments.Contains($envName.ToLower())){
        Write-Host "Invalid entry! Please choose one of the following: " $environments.Keys
        return
    } 

    if($version -eq 15) {
        $global:ver = $v15
    }

    $global:baseURI = $environments[$envName.ToLower()]
    $global:uri = $global:baseURI + $global:ver

    Write-Host "Set environment: " $global:uri
}

#####################
# LevelUp API Calls #
#####################

## Authenticate ##
function Get-LevelUpAccessToken([string]$apikey, [string]$username, [string]$password) {
    $tokenRequest = @{ "access_token" = @{ "client_id" = $apikey; "username" = $username; "password" = $password } }

    $body = $tokenRequest | ConvertTo-Json

    $theURI = $global:uri + "access_tokens"
    
    $response = Submit-PostRequest $theURI $body $headers

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.access_token
}

function Set-LevelUpAuthorizationHeader([string]$token) {

    $authKey = "Authorization"
    $tokenString = "token"

    if($headers.ContainsKey($authKey)) {
        $headers[$authKey] = "$tokenString $token"
    } else {
        $headers.Add("Authorization", "$tokenString $token")
    }
}

## Get Merchant Locations ##
function Get-LevelUpMerchantLocations([int]$merchantId) {
    $theURI = $global:uri + "merchants/$merchantId/locations"

    $response = Submit-GetRequest $theURI $headers

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed
}

## Create Order ##
function Submit-LevelUpOrder([int]$locationId, [string]$qrCode, [int]$spendAmount) {
    return Submit-LevelUpOrder $locationId $qrCode $spendAmount $null $null
}

function Submit-LevelUpOrder([int]$locationId, [string]$qrCode, [int]$spendAmount, [Nullable[int]]$appliedDiscount) {
    return Submit-LevelUpOrder $locationId $qrCode $spendAmount $appliedDiscount $null
}

function Submit-LevelUpOrder([int]$locationId, [string]$qrCode, [int]$spendAmount, [Nullable[int]]$appliedDiscount, [Nullable[int]]$availableGiftCard) {
    return Submit-LevelUpOrder $locationId $qrCode $spendAmount $appliedDiscount $availableGiftCard $false
}

function Submit-LevelUpOrder([int]$locationId, [string]$qrCode, [int]$spendAmount, [Nullable[int]]$appliedDiscount, [Nullable[int]]$availableGiftCard, [bool]$partialAuthAllowed) {
    
    $theURI = $global:uri + "orders"

    $item1 = Format-LevelUpItem "Sprockets" "Lovely sprockets with gravy" "Weird stuff" "4321" "1234" 0 0 7
    $item2 = Format-LevelUpItem "Soylent Green Eggs & Spam" "Highly processed comestibles" "Food. Or perhaps something darker..." "0101001" "55555" 100 100 1 

    $order = @{ 
      "order" = @{ 
        "location_id" = $locationId; 
        "payment_token_data" = $qrCode;
        "spend_amount" = $spendAmount;
        "applied_discount_amount" = $appliedDiscount;
        "available_gift_card_amount" = $availableGiftCard;
        "identifier_from_merchant" = "Check #TEST";
        "cashier" = "LevelUp Powershell Script";
        "register" = "3.14159";
        "partial_authorization_allowed" = $partialAuthAllowed;
        "items" = @( 
          $item1, 
          $item2 
        );
      }
    }          

    $body = $order | ConvertTo-Json -Depth 5
    
    $response = Submit-PostRequest $theURI $body $headers

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.order
}

## Refund Order ##
function Undo-LevelUpOrder([string]$orderId) {
    $theURI = "{0}orders/{1}/refund" -f $global:uri, $orderId

    $refundRequest = @{"refund" = @{"manager_confirmation" = $null } }
    $body = $refundRequest | ConvertTo-Json

    $response = Submit-PostRequest $theURI $null $headers
    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.order
}

## Gift Card Add Value ##
function Add-LevelUpGiftCardValue([int]$merchantId, [string]$qrData, [int]$amountToAdd) {
    
    $theUri = "{0}merchants/{1}/gift_card_value_additions" -f ($global:baseURI + $v15), $merchantId

    $addValueRequest = @{ "gift_card_value_addition" = @{ "payment_token_data" = $qrData; "value_amount" = $amountToAdd } }
    $body = $addValueRequest | ConvertTo-Json

    $response = Submit-PostRequest $theUri $body $headers

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.gift_card_value_addition
}

## Gift Card Destroy Value ##
function Remove-LevelUpGiftCardValue([int]$merchantId, [string]$qrData, [int]$amountToDestroy) {
    $theUri = "{0}merchants/{1}/gift_card_value_removals" -f ($global:baseURI + $v15), $merchantId
    
    $destroyValueRequest = @{ "gift_card_value_removal" = @{ "payment_token_data" = $qrData; "value_amount" = $amountToDestroy } }
    $body = $destroyValueRequest | ConvertTo-Json

    $response = Submit-PostRequest $theUri $body $headers

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.gift_card_value_removal
}

## Detached Refund ##
function Add-LevelUpMerchantFundedCredit([int]$locationId, [string]$qrData, [int]$amountToAdd) {
    $theUri = "{0}detached_refunds" -f $global:uri

    $detachedRefundRequest = @{ 
        "detached_refund" = @{
            "cashier" = "LevelUp Powershell Script";
            "credit_amount" = $amountToAdd;
            "customer_facing_reason" = "Sorry about your coffee!";
            "identifier_from_merchant" = "123abc";
            "internal_reason" = "Customer did not like his coffee";
            "location_id" = $locationId;
            "manager_confirmation" = $null;
            "payment_token_data" = $qrData;
            "register" = "3"
            }
         }
    $body = $detachedRefundRequest | ConvertTo-Json

    $response = Submit-PostRequest $theUri $body $headers

    $parsed = $response | ConvertFrom-Json

    return $parsed.detached_refund
}

## Get Available Merchant Credit ##
function Get-LevelUpMerchantCredit([int]$locationId, [string]$qrCode) {

    $theUri = "{0}locations/{1}/merchant_funded_credit?payment_token_data={2}" -f $global:uri, $locationId, $qrCode

    $response = Submit-GetRequest $theUri $headers

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.merchant_funded_credit
}

## Get Recent Orders At Location ##
function Get-LevelUpOrdersByLocation([int]$locationId) {
    $theURI = "{0}locations/{1}/orders" -f $global:uri, $locationId

    $response = Submit-GetRequest $theURI $headers

    $parsed = $response | ConvertFrom-Json

    return $parsed
}

## Get Order Details ##
function Get-LevelUpOrderDetailsForMerchant([int]$merchantId, [string]$orderId) {
    
    $theURI = "{0}merchants/{1}/orders/{2}" -f $global:uri, $merchantId, $orderId

    $response = Submit-GetRequest $theURI $headers

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.order
}

################
# REST Methods #
################
function Submit-GetRequest([string]$uri, $headers) {
    return iwr -Method Get -Uri $uri -Headers $headers
}

function Submit-PostRequest([string]$uri, [string]$body, $headers) {
    return iwr -Method Post -Uri $uri -Body $body -Headers $headers
}

function Submit-PutRequest([string]$uri, [string]$body, $headers) {
    return iwr -Method Put -Uri $uri -Body $body -Headers $headers
}

##################
# Helper Methods #
##################
function Format-LevelUpItem([string]$name, [string]$description, [string]$category, [string]$upc, [string]$sku, [int]$unitPriceAmount, [int]$chargedPriceAmount, [int]$quantity) {
    $item = @{ 
      "item" = @{ 
        "name" = $name; 
        "description" = $description; 
        "category" = $category; 
        "upc" = $upc; 
        "sku" = $sku; 
        "unit_price_amount" = $unitPriceAmount; 
        "charged_price_amount" = $chargedPriceAmount; 
        "quantity" = $quantity;
      }
    }

    return $item;
}

function Redact-LevelUpQrCode([string]$qrCode) {
    $startIndex = 11
    $tokenLength = 13

    if($qrCode.Length -le ($startIndex + $tokenLength)){
        return $qrCode
    } else {
        return $qrCode.Replace($qrCode.Substring($startIndex, $tokenLength), "[** Redacted **]")
    }
}
