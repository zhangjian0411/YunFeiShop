$devCertsDir = "../misc/dev-certs"

Get-ChildItem -Path "$devCertsDir/*.crt" | ForEach-Object {
    $certPath = $_.FullName
    $keyPath = $devCertsDir + "/" + $_.BaseName + ".key"

    if (Test-Path $keyPath) {
        $tlsSecretName = $_.BaseName + "-tls"

        kubectl delete secret $tlsSecretName --ignore-not-found     # delete secret first to ensure the secret will be created with new value
        kubectl create secret tls $tlsSecretName --cert=$certPath --key=$keyPath
    }
}