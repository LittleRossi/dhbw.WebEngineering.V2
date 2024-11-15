# dhbw.WebEngineering Projekt

## Docker Compose

### Docker Image

Das Docker-Image der api liegt auf der ghcr und ist unter

```docker
ghcr.io/littlerossi/biletado-api-arm:latest
ghcr.io/littlerossi/biletado-api-amd:latest
```

zu erreichen.

### Variablen

Das Compose File kann mit einer vielzahl an Variablen gefüllt werden um die internen Einstellungen der API zu verändern.

```yaml
- POSTGRES_ASSETS_PORT
- POSTGRES_ASSETS_HOST
- POSTGRES_ASSETS_DBNAME
- POSTGRES_ASSETS_PASSWORD
- POSTGRES_ASSETS_USER
- KEYCLOAK_REALM
- KEYCLOAK_HOST
- KEYCLOAK_PORT
- KEYCLOAK_AUDIENCE
- Serilog__MinimumLevel=Information
- Serilog__WriteTo__1__Args__path=logs/MyAppLog-.txt
```

### Beispiel eines Compose Files

```bash
services:
  api:
    container_name: biletado-api
    environment:
      - POSTGRES_ASSETS_PORT=5432
      - POSTGRES_ASSETS_HOST=localhost
      - POSTGRES_ASSETS_DBNAME=assets_v3
      - POSTGRES_ASSETS_PASSWORD=postgres
      - POSTGRES_ASSETS_USER=postgres
      - KEYCLOAK_REALM=biletado
      - KEYCLOAK_HOST=localhost
      - KEYCLOAK_PORT=9090
      - KEYCLOAK_AUDIENCE=account
      - Serilog__MinimumLevel=Information
      - Serilog__WriteTo__1__Args__path=logs/MyAppLog-.txt
    ports:
      - 8080:8080
    image: ghcr.io/littlerossi/biletado-api-arm:latest

```

### Ausführen des Compose files

Um mit Podman die API hochzufahren, muss folgender Befehl im Ordner des Compose-Files ausgeführt werden:

```bash
podman compose --file .\docker-compose.yaml up
```

oder wahlweise wenn man nicht mit den Logs des Containers verbunden sein will über:

```bash
podman compose --file .\docker-compose.yaml up --detach
```

### Datenbank lokal als Docker Container laufen lassen

Um die DB lokal laufen zu lassen, kann folgender Befehl verwendet werden. Hier sind Tabellen und co. schon erstellt:

```bash
podman run --name mypostgresdb -p 5432:5432 ghcr.io/littlerossi/biletado-database:latest
```

## Logging

Um das Logging umzusetzen wurde auf das NugetPaket Serilog gesetzt. Das Loglevel von Serilog kann unterschiedlich eingestellt werden. Dies geht über die Environment Variable **Serilog\_\_MinimumLevel** im Compose-File.

Hierbei stehen einem mehrere Möglichkeiten zur Verfügung:

```bash
- Serilog__MinimumLevel=Verbose
- Serilog__MinimumLevel=Debug
- Serilog__MinimumLevel=Information
- Serilog__MinimumLevel=Warning
- Serilog__MinimumLevel=Error
- Serilog__MinimumLevel=Fatal
```

Für genauere Informationen siehe: [Dokumentation](https://github.com/serilog/serilog/wiki/Configuration-Basics#minimum-level)

## Authentifizierung

Die Authentifizierung läuft über einen KeyCloak Server. Hierfür müssen folgenden Environment Variables im Compose File gesetzt werden:

```bash
- KEYCLOAK_REALM
- KEYCLOAK_HOST
- KEYCLOAK_PORT
- KEYCLOAK_AUDIENCE
```

### lokales Erstellen eines JWT Token

Um beispielweise in Swagger einen Bearer Token eintragen zu können, muss über die UI der lokal laufenden biletado Instanz gegangen werden.

Hierzu zunächst auf _http://localhost:9090/_ gehen und dort im Unterpunkt **Access Token** auf **open on jwt.io** klicken und dort den **Encoded** Token kopieren und in Swagger oder Postman, etc. eintragen.

## Getting Started

Um das Projekt lokal debuggen zu können, muss eine Instanz von biletado laufen. Dies ist notwendig, da hierdurch Keycloak und die Datenbank bereitgestellt wird.

### Starten der lokalen biletado instanz

1. Falls noch nicht vorhanden, muss in Podman ein **kind Cluster** erstellt werden
2. Anschließen folgenden Befehl ausführen, diser fährt das Cluster hoch:

```bash
kubectl create namespace biletado
kubectl config set-context --current --namespace biletado
kubectl apply -k https://gitlab.com/biletado/kustomize.git//overlays/kind?ref=main --prune -l app.kubernetes.io/part-of=biletado -n biletado
kubectl rollout status deployment -n biletado -l app.kubernetes.io/part-of=biletado --timeout=600s
kubectl wait pods -n biletado -l app.kubernetes.io/part-of=biletado --for condition=Ready --timeout=120s
```

3. Jetzt muss noch ein Port-Forwarding eingestellt werden, sodass die lokale API auf die Datenbank im Cluster zugreifen kann. Hierzu folgenden Befehl ausführen:

```bash
kubectl port-forward service/postgres 5432:5432
```

Jetzt greift die API auf die Datenbank im Cluster zu und kann getestet / entwickelt werden.

## Integrationstests

### Integrationstests mitteles IntelliJ HTTP Client

Um die Integrationstests laufen zu lassen ist intelliJ notwendig. Hier zu muss in intelliJ das Projekt \*_dhbw.WebEngineering.IntegrationsTests/apidocs-main_ gestartet werden.

Jetzt muss mithilfe eines Podman Containers die validate-assets-v3.js Datei erstellt werden. Hierzu folgenden Befehl im **apidocs-main** Ordner ausführen:

```bash
podman run --rm -i -t \
    -e DIRECTORY=/app/openapi/v3
    --security-opt="label=disable"
    -v $(pwd)/public:/app/openapi
    registry.gitlab.com/http-client-schema-check/http-client-schema-check:latest
```

Für Windows:

```bash
podman run --rm -i -t `
    -e DIRECTORY=/app/openapi/v3 `
    --security-opt="label=disable" `
    -v ${PWD}/public:/app/openapi `
    registry.gitlab.com/http-client-schema-check/http-client-schema-check:latest
```

Anschließend müssen ggf. in der **http-client.env.json** der Port der API angepasst werden auf den der lokal laufenden API.

Bevor die Tests ausgeführt werden können, muss zunächst die 000_Authenticate.http ausgeführt werden. Auch hier als Environment: **kind** auswählen. Hiermit wird der notwendige Authentifizierungs-Key von keycloak gezogen.

Jetzt können die Tests durchgeführt werden. Hierbei sind nur die folgenden zwei Testdateien notwendig:

```bash
200_ReadAssets-v3.http
300_WriteAssets-v3.http
```

### Eigenentwickelte Unit Tests

Für die Entwicklung der Unit-Tests wurde das die Mocking-Libraries Moq eingesetzt.

Um die Tests zu starten, im root Verzeichnis dotnet test ausführen.
```bash
dotnet test
```

## Pipeline

Die Pipeline wurde in Form einer GitHub-Action (Workflow) realisiert. Der Workflow führt die Unit Tests durch und erstellt danach das Docker-Image der API und veröffentlicht dieses auf die GHCR.

## Biletado mit eigener API als Cluster betreiben

Die angepasste _kustomization.yaml_ liegt im Ordner: **dhbw.WebEngineering.V2.Kustomize/kustomization.yaml**

```bash
apiVersion: kustomize.config.k8s.io/v1beta1
kind: Kustomization

resources:
  - https://gitlab.com/biletado/kustomize.git//overlays/kind

patches:
  - patch: |-
      - op: replace
        path: /spec/template/spec/containers/0/image
        value: ghcr.io/littlerossi/biletado-api-arm:latest
      - op: replace
        path: /spec/template/spec/containers/0/ports/0/containerPort
        value: 8080
    target:
      group: apps
      version: v1
      kind: Deployment
      name: assets
```

1. Schritt: Hochfahren des originalen Biletado Clusters

```bash
kubectl create namespace biletado
kubectl config set-context --current --namespace biletado
kubectl apply -k https://gitlab.com/biletado/kustomize.git//overlays/kind?ref=main --prune -l app.kubernetes.io/part-of=biletado -n biletado
kubectl rollout status deployment -n biletado -l app.kubernetes.io/part-of=biletado --timeout=600s
kubectl wait pods -n biletado -l app.kubernetes.io/part-of=biletado --for condition=Ready --timeout=120s
```

2. Schritt: API (assets) "austauschen"

Folgenden Befehl ausführen im Ordner wo die kustomization.yaml liegt (**dhbw.WebEngineering.V2.Kustomize/**)

```bash
# execute this in the folder with kustomization.yaml
kubectl apply -k . --prune -l app.kubernetes.io/part-of=biletado -n biletado
kubectl wait pods -n biletado -l app.kubernetes.io/part-of=biletado --for condition=Ready --timeout=120s
```

## Hintergrund

Dieses Projekt wurde im Rahmen der Vorlesung Web Engineering 2 an der DHBW Karlsruhe im 5. Semester erstellt.

### Wichtige Links

- (Repo) *https://gitlab.com/biletado*
- (API-Docs) *https://demo.biletado.org/rapidoc/*
- (demo Instanz) *https://demo.biletado.org/*