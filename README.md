# CarbonScaler

[![C#](https://img.shields.io/badge/C%23-%2300599C.svg?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/) 
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white) ![Git](https://img.shields.io/badge/git-%23F05033.svg?style=for-the-badge&logo=git&logoColor=white)
![Linux](https://img.shields.io/badge/Linux-FCC624?style=for-the-badge&logo=linux&logoColor=black) ![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white)

       ______           __               _____            __          ____  ____  ______
      / ____/___ ______/ /_  ____  ____ / ___/_________ _/ /__  _____/ __ \/ __ \/ ____/
     / /   / __ `/ ___/ __ \/ __ \/ __ \\__ \/ ___/ __ `/ / _ \/ ___/ /_/ / / / / /     
    / /___/ /_/ / /  / /_/ / /_/ / / / /__/ / /__/ /_/ / /  __/ /  / ____/ /_/ / /___   
    \____/\__,_/_/  /_.___/\____/_/ /_/____/\___/\__,_/_/\___/_(_)/_/    \____/\____/   


                     
# Duurzaam schalen voor beginners
Termen als duurzaamheid/sustainability, carbonfootprint, CO2 neutraal en klimaatverandering zijn niet meer weg te denken uit het hedendaagse debat over milieubescherming en de transitie naar een duurzamere wereld. Dat iedereen zijn steentje bijdraagt om deze planeet zo leefbaar mogelijk te houden is een mooi en nobel streven. Bij CO2 uitstoot denk je wellicht snel aan dampende fabrieken, zware vrachtauto's of landbouw, maar ook wij als ICT sector met onze datacenters dragen bij een CO2 uitstoot. (Zie b.v. dit artikel in [Science](https://datacenters.lbl.gov/sites/default/files/Masanet_et_al_Science_2020.full_.pdf) of het onderzoek van [Internal Energy Agency](https://www.iea.org/energy-system/buildings/data-centres-and-data-transmission-networks)). Meerdere onderzoeken wijzen uit dat onze sector verantwoordlijk is voor 1 tot 2 procent van de totale uitstoot. Dit klinkt wellicht weinig maar als we uitgaan van de 2% uitstoot is dit alsnog 736 miljoen ton. Dit in vergelijking met de gemiddelde uitstoot van 1 persoon per jaar in de EU wat neerkomt op 6 to 8 ton is het 736 miljoen ton een aanzienlijke bijdrage.

## Want kun je doen?
Het terugdringen van deze uitstoot klinkt misschien als een druppel op een gloeiende plaat, maar elke druppel is er één. In het kader van deze druppels zijn er legio manieren waarop je als organisatie de het energie verbruik in in je datacenters kan verminderen denk b.v. afsluiten van een contract bij een data center met een duidelijke "groene" visie wat betreft hernieuwbare energie, koeling faciliteiten en effienctie van hardware, maar denk bijvoorbeeld ook eens aan het uitzetten van machines op uren dat ze niet nodig zijn (b.v. s'nachts). 
### The next level
Naast deze wat meer voor de hand liggende zaken zijn er ook geavanceerdere manieren om de CO2 uitstoot van bepaalde onderdelen van je applicatielandschap te verbeteren. Een mooi voorbeeld hiervan is het dynamisch gebruik maken van de realtime data over de huidige energie mix. Energie zoals wij deze gebruiken is altijd een samenvoegsel van verschillende energie bronnen denk dan aan: gas, kolen, nucleair, wind en zon. Vooral de laatste 2 energie bronnen zijn natuurlijk niet altijd in dezelfde hoeveelheden aanwezig (b.v. geen wind en bewolking). Dit betekend dan ook dat de energie op verschillende momenten op de dag "groener" is dan op andere momenten. 

Een aantal partijen bieden deze realtime data aan om gebruik van te maken binnen je organisatie. Dit zijn b.v. [ElectricityMaps](https://app.electricitymaps.com/map) en [Watttime](https://watttime.org/docs-dev/coverage-map/)

### Schaalbaarheid scenario's
Binnen veel organisaties zijn er bepaalde services/applicaties/achtergrond processen die wel van belang zijn maar het tijdstip waarop deze draaien zijn minder van belang. Je kunt hier bijvoorbeeld denken aan bepaalde calculaties, rapportages of data synchronisatie. Door de gebruik te maken van de realtime data van electricitymap of watttime kun je de keuze maken om deze processen op te schalen op het moment dat de energie mix groen genoeg is voor jou maatstaven. 

Door gebruik te maken van het autoscaling platform KEDA in combinatie met de data van electricitymap of watttime kun je er voor zorgen dat je processen vol automisch worden opgeschaald. 

**Om deze constructie te illustreren is er de volgende [git repo](https://github.com/pabes74/CarbonScaler) beschikbaar gemaakt.** Deze repo bevat een voorbeeld "proof of concept" van een carbonscaler container die de data uit ElectricityMaps leest deze omzet naar een raportcijfer en op basis hiervan een MockBackgroundProcess container schaalt naar meerdere instanties. Hier wordt gebruik gemaakt van KEDA en Azure Container App (zie [deze blog](https://www.bergler.nl/container-orchestratie-gemakkelijk-gemaakt-maar-hoe-dan/)). Deze laatste keuze impliceert dan je ben hier gebonden het gebruik van de Azure cloud¹. Aangezien Azure Container App intern gewoon kubernetes gebruik kun je ook gebruik maken van KEDA. 

> *¹ Microsoft is voornemens om per 2025 volledig klimaat neutraal te zijn, dit doet de vraag rijzen hoe nuttig dit is in Azure na 2025. Maar er is een kanttekening dat onbekend is of dit betekend dat ze ook 100% groene energie gebruiken of ook gedeeltelijk nog CO-2 compenseren. 100% groene energie leveren op basis van zon en wind is lastig in Nederland*

Schematisch zie dit Proof of concept er als volgt uit:

![carbonscaler](CarbonScaler.png)
Je ziet hier dat de carbonscaler een API call doet naar Electricity Maps voor het ophalen van de recente energy mix. 
In de repo is dit opgenomen in de ElectriticyMapService.cs deze service doet een API call op basis van de volgende appsettings:

```json
  "ElectricityMapService": {
    "BaseAddress": "https://api.electricitymap.org/",
    "AuthToken": "##########",
    "CarbonIntensityEndpoint": "v3/carbon-intensity/latest",
    "Zone": "NL"
  }
```
Hier moet uiteraard gebruik worden gemaakt van je eigen AuthToken die je kunt aanvragen bij ElectricityMap. Tevens is hier zone NL aangegeven, afhankelijk van de locatie van je data centrum kun je dit ook aanpassen.

De mockbackground container app heeft de KEDA scaler metrics API geconfigureerd (zie [docs](https://keda.sh/docs/2.15/scalers/metrics-api/)). Deze kijkt naar de uitkomsten van de carbonscaler en schaalt op basis van de geconfigureerde targetvalue. 
In de deployment bicep van je container app zou je bijvoorbeeld onstaande kunnen opnemen om de KEDA scaling te configureren:
```yaml
      scale: {
        minReplicas: 1
        maxReplicas: 5
        rules: [
          {
            name: 'carbonscaler'
            custom: {
              type: 'metrics-api'
              metadata: {
                format: 'json'
                targetValue: '6'
                unsafeSsl: 'true'
                url: 'https://carbondata***/api/electricitymap'
                valueLocation: 'carbonGrade'
              }
            }
          }
        ]
```
Dit is voldoende om er voor te zorgen dat KEDA scaler actief is en de gekoppelde services (in dit geval de mockbackground service) gaat schalen op basis van de targetValue. Tevens is hier opgenomen dat de scaling een maximum heeft van 5 replicas en een minimum van 1. KEDA heeft de mogelijkheid te schalen vanaf 0 tot n. Let wel op dat hoe meer replica's hoe hoger de hostig kosten.


## Conclusie
Dit is één van de voorbeeld maatregelen die je als organisatie kunt treffen om je CO2 footprint te verkleinen. Zoals eerder aangegeven zijn dit kleine stappen maar elk begin is er één. Je kunt met deze technologie ook maatregelen treffen om bijvoorbeeld bepaalde services te schalen in andere regio's. Zo is de energie mix in scandinavie in veel gevallen veel "groener" dan in nederland. De effort die het kost om een dergelijk constructie op te zetten is relatief laag en daarmee het overwegen binnen organisaties waard. Hopelijk geeft dit artikel wat inspiratie om eens serieuze overwegingen te maken in de richting van duurzamere manieren van hosting.
                                                               

## How to Start

#### Local build
```sh
docker build -f Dockerfile -t carbondata:dev .
docker build -f Dockerfile -t mockbackgroundprocess:dev .
```
#### Local run
```sh
docker run -it -p 3084:8080 carbondata:dev
docker run -it -p 3085:8080 mockbackgroundprocess:dev
```

#### Connect to Container registry
```sh
docker login ***.azurecr.io
username: ***
```

#### Push local container registry
```sh
docker tag carbondata:dev ***.azurecr.io/carbondata
docker push ***.azurecr.io/carbondata
docker tag mockbackgroundprocess:dev ***.azurecr.io/mockbackgroundprocess
docker push ***.azurecr.io/mockbackgroundprocess
```

## Test service in cloud
In console van de mockbackgroundprocess

#### Install packages
```sh
apt-get update
apt-get upgrade
apt install curl
```

#### Test service endpoints
```sh
curl carbondata/api/ElectricityMap
curl carbondata/api/stub
```
