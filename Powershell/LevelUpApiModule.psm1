## PowerShell module for LevelUp API
## Copyright(c) 2016 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.

## Base Uris ##
$productionBaseURI = "https://api.thelevelup.com/"
$stagingBaseURI = "https://api.staging-levelup.com/"
$sandboxBaseURI = "https://sandbox.thelevelup.com/"

#################
## LEVELUP API ##
#################

$v14 = "v14/"
$v15 = "v15/"

$global:ver = $v14
$global:baseURI = $productionBaseURI
$global:uri = $global:baseURI + $global:ver

# Common HTTP Headers not including Authorization Header
$commonHeaders = @{"Content-Type" = "application/json"; "Accept" = "application/json"}

# Store the merchant access token here
$Script:merchantAccessToken = ''

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

    $response = Submit-PostRequest $theURI $body $commonHeaders

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.access_token
}

# Add Authorization header to common headers and return new headers
function Add-LevelUpAuthorizationHeader([string]$token, $headers = $commonHeaders) {

    $authKey = "Authorization"
    $tokenString = "token"

    $newHeaders = @{}
    if ($headers -ne $null) {
        $newHeaders += $headers
    }

    if($newHeaders.ContainsKey($authKey)) {
        $newHeaders[$authKey] = "$tokenString $token"
    } else {
        $newHeaders.Add($authKey, "$tokenString $token")
    }
    $newHeaders
}

# Set the LevelUp acceess token value used within the Authorization Header
function Set-LevelUpAccessToken([string]$token) {
    $Script:merchantAccessToken = $token
}

## Get Merchant Locations ##
function Get-LevelUpMerchantLocations([int]$merchantId) {
    $theURI = $global:uri + "merchants/$merchantId/locations"

    $response = Submit-GetRequest $theURI $merchantAccessToken

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed
}

## Create Proposed Order ##
function Submit-LevelUpProposedOrder {

    [CmdletBinding()]
    Param(
        [Parameter(Mandatory=$true)]
        [int]$locationId,

        [Parameter(Mandatory=$true)]
        [string]$qrCode,

        [Parameter(Mandatory=$true)]
        [int]$spendAmount,

        [int]$taxAmount=0,

        [bool]$partialAuthAllowed=$true
    )

    $theURI = $global:baseURI + $v15 + "proposed_orders"

    $proposed_order = @{
      "proposed_order" = @{
        "location_id" = $locationId;
        "payment_token_data" = $qrCode;
        "spend_amount" = $spendAmount;
        "tax_amount" = $taxAmount;
        "identifier_from_merchant" = "Check #TEST";
        "cashier" = "LevelUp Powershell Script";
        "register" = "3.14159";
        "partial_authorization_allowed" = $partialAuthAllowed;
        "items" = Get-LevelUpSampleItemList;
      }
    }

    $body = $proposed_order | ConvertTo-Json -Depth 5

    $accessToken = "merchant=" + $merchantAccessToken

    $response = Submit-PostRequest $theURI $body $accessToken
    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.proposed_order
}

## Complete Order **
function Submit-LevelUpCompleteOrder {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory=$true)]
        [int]$locationId,

        [Parameter(Mandatory=$true)]
        [string]$qrCode,

        [Parameter(Mandatory = $true)]
        [int]$spendAmount,

        [Parameter(Mandatory=$true)]
        [string]$proposedOrderUuid,

        [Nullable[int]]$appliedDiscount=$null,

        [int]$taxAmount=0,

        [bool]$partialAuthAllowed=$true,

        [int]$exemptionAmount=0
    )

    $theURI = $global:baseURI + $v15 + "completed_orders"

    $completed_order = @{
      "completed_order" = @{
        "location_id" = $locationId;
        "payment_token_data" = $qrCode;
        "proposed_order_uuid" = $proposedOrderUuid;
        "applied_discount_amount" = $appliedDiscount;
        "spend_amount" = $spendAmount;
        "tax_amount" = $taxAmount;
        "exemption_amount" = $exemptionAmount;
        "identifier_from_merchant" = "Check #TEST";
        "cashier" = "LevelUp Powershell Script";
        "register" = "3.14159";
        "partial_authorization_allowed" = $partialAuthAllowed;
        "items" = Get-LevelUpSampleItemList;
      }
    }

    $body = $completed_order | ConvertTo-Json -Depth 5

    $accessToken = "merchant=" + $merchantAccessToken
    $response = Submit-PostRequest $theURI $body $accessToken

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.order
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
        "items" = Get-LevelUpSampleItemList;
      }
    }

    $body = $order | ConvertTo-Json -Depth 5

    # Access token for orders endpoint depends on API version
    $accessToken = $merchantAccessToken
    if ($ver -ne $v14) {
        $accessToken = 'merchant=' + $merchantAccessToken
    }

    $response = Submit-PostRequest $theURI $body $accessToken

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.order
}

## Refund Order ##
function Undo-LevelUpOrder([string]$orderId) {
    $theURI = "{0}orders/{1}/refund" -f $global:uri, $orderId

    $refundRequest = @{"refund" = @{"manager_confirmation" = $null } }
    $body = $refundRequest | ConvertTo-Json

    # Access token for refund endpoint depends on API version
    $accessToken = $merchantAccessToken
    if ($ver -ne $v14) {
        $accessToken = "merchant=" + $merchantAccessToken
    }

    $response = Submit-PostRequest $theURI $null $accessToken

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.order
}

## Gift Card Add Value ##
function Add-LevelUpGiftCardValue([int]$merchantId, [string]$qrData, [int]$amountToAdd) {

    $theUri = "{0}merchants/{1}/gift_card_value_additions" -f ($global:baseURI + $v15), $merchantId

    $addValueRequest = @{ "gift_card_value_addition" = @{ "payment_token_data" = $qrData; "value_amount" = $amountToAdd } }
    $body = $addValueRequest | ConvertTo-Json

    $response = Submit-PostRequest $theUri $body $merchantAccessToken

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.gift_card_value_addition
}

## Gift Card Destroy Value ##
function Remove-LevelUpGiftCardValue([int]$merchantId, [string]$qrData, [int]$amountToDestroy) {
    $theUri = "{0}merchants/{1}/gift_card_value_removals" -f ($global:baseURI + $v15), $merchantId

    $destroyValueRequest = @{ "gift_card_value_removal" = @{ "payment_token_data" = $qrData; "value_amount" = $amountToDestroy } }
    $body = $destroyValueRequest | ConvertTo-Json

    $response = Submit-PostRequest $theUri $body $merchantAccessToken

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

    # Access token for depends on API version
    $accessToken = $merchantAccessToken
    if ($ver -ne $v14) {
        $accessToken = "merchant=" + $merchantAccessToken
    }

    $response = Submit-PostRequest $theUri $body $accessToken

    $parsed = $response | ConvertFrom-Json

    return $parsed.detached_refund
}

## Get Available Merchant Gift Card Credit ##
function Get-LevelUpMerchantGiftCardCredit([int]$locationId, [string]$qrCode) {
    $theUri = "{0}locations/{1}/get_merchant_funded_gift_card_credit" -f ($global:baseURI+$v15), $locationId

    $merchantFundedGiftCardRequest = @{
        "get_merchant_funded_gift_card_credit" = @{
            "payment_token_data" = $qrCode
        }
    }

    $body = $merchantFundedGiftCardRequest | ConvertTo-Json

    $accessToken = "merchant=" + $merchantAccessToken

    $response = Submit-PostRequest $theUri $body $accessToken

    $parsed = $response | ConvertFrom-Json

    return $parsed.merchant_funded_gift_card_credit
}

## Get Available Merchant Credit ##
function Get-LevelUpMerchantCredit([int]$locationId, [string]$qrCode) {

    $theUri = "{0}locations/{1}/merchant_funded_credit?payment_token_data={2}" -f $global:uri, $locationId, $qrCode

    # Access token for depends on API version
    $accessToken = $merchantAccessToken
    if ($ver -ne $v14) {
        $accessToken = "merchant=" + $merchantAccessToken
    }

    $response = Submit-GetRequest $theUri $accessToken

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.merchant_funded_credit
}

## Get Recent Orders At Location ##
function Get-LevelUpOrdersByLocation([int]$locationId) {
    # v14 only!
    $theURI = "{0}locations/{1}/orders" -f $global:baseURI+$v14, $locationId

    $response = Submit-GetRequest $theURI $merchantAccessToken

    $parsed = $response | ConvertFrom-Json

    return $parsed
}

## Get Order Details ##
function Get-LevelUpOrderDetailsForMerchant([int]$merchantId, [string]$orderId) {

    $theURI = "{0}merchants/{1}/orders/{2}" -f $global:uri, $merchantId, $orderId

    $response = Submit-GetRequest $theURI $merchantAccessToken

    $parsed = $response.Content | ConvertFrom-Json

    return $parsed.order
}

################
# REST Methods #
################
function Submit-GetRequest([string]$uri, [string]$accessToken=$null, $headers=$commonHeaders) {
    $theHeaders = $headers
    # Add HTTP Authorization header if access token specified
    if ($accessToken -ne $null) {
        $theHeaders = Add-LevelUpAuthorizationHeader $accessToken $headers
    }

    try {
        return iwr -Method Get -Uri $uri -Headers $theHeaders
    }
    catch [System.Net.WebException] {
        HandleWebException($_.Exception)
    }
}

function Submit-PostRequest([string]$uri, [string]$body, [string]$accessToken=$null, $headers=$commonHeaders) {
    $theHeaders = $headers
    # Add HTTP Authorization header if access token specified
    if ($accessToken -ne $null) {
        $theHeaders = Add-LevelUpAuthorizationHeader $accessToken $headers
    }

    try {
        return iwr -Method Post -Uri $uri -Body $body -Headers $theHeaders
    }
    catch [System.Net.WebException] {
        HandleWebException($_.Exception)
    }
}

function Submit-PutRequest([string]$uri, [string]$body, [string]$accessToken=$null, $headers=$commonHeaders) {
    $theHeaders = $headers
    # Add HTTP Authorization header if access token specified
    if ($accessToken -ne $null) {
        $theHeaders = Add-LevelUpAuthorizationHeader $accessToken $headers
    }

    try {
        return iwr -Method Put -Uri $uri -Body $body -Headers $theHeaders
    }
    catch [System.Net.WebException] {
        HandleWebException($_.Exception)
    }
}

##################
# Helper Methods #
##################

function Get-LevelUpSampleItemList() {
    $item1 = Format-LevelUpItem "Sprockets" "Lovely sprockets with gravy" "Weird stuff" "4321" "1234" 0 0 7
    $item2 = Format-LevelUpItem "Soylent Green Eggs & Spam" "Highly processed comestibles" "Food. Or perhaps something darker..." "0101001" "55555" 100 100 1

    $items = @($item1,$item2);
    return $items;
}

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

function HandleWebException([System.Net.WebException]$exception) {
    $statusCode = [int]$exception.Response.StatusCode
    $statusDescription = $exception.Response.StatusDescription
    Write-Host -ForegroundColor:Red "HTTP Error: $statusCode $statusDescription"

    # Get the response body as JSON
    $responseStream = $exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($responseStream)
    $global:responseBody = $reader.ReadToEnd()
    Write-Host "Error message:"
    try {
        $json = $global:responseBody | ConvertFrom-JSON
        $json | ForEach-Object { Write-Host  "    " $_.error.message }
    }
    catch {
        # Just output the body as raw data
        Write-Host -ForegroundColor:Red $global:responseBody
    }
    break
}