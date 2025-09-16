# Contributing Guidelines
## Main Branch
De main branch is de branch waar een stabiele en release-klare versie van de code in staat. Feature branches mogen dus niet direct naar main mergen!
## Develop (dev) Branch
- Dit is de branch waar feature branches aan worden toegevoegd en samengevoegd wanneer deze ready zijn
- Is de branch waar actieve ontwikkeling in wordt gedaan.
## Feature Branches
- Hier worden nieuwe functionaliteiten volgens Use Cases ontwikkelt
- Een feature branch heeft dan ook altijd "feature", de use case en de feature zelf in de naam: "feature/uc#-winkelmand"
- Een feature branch mag alleen mergen naar de dev branch via een pull request, niet naar de main of een release branch. 
## Release Branches
- Worden alleen aangemaakt wanneer een nieuwe versie wordt voorbereid
- Naam van de branches bevat altijd het "release" gevolgd door het versie nummer van de release: "release/v0.5"
- Worden aangemaakt vanuit de develop branch
- Release wordt naar main gemerged via een pull request
## Hotfix Branches
- Worden gebruikt om kritieke bugs in productie op te lossen
- Wordt aangemaakt vanuit en gemerged met main
- Na het mergen met main worden de updates gelijk doorgevoerd naar de dev, feature en release branches waar nodig.
- Naam van de branch moet altijd "hotfix", het versie nummer en de bugfix bevatten: "hotfix/v0.5.1-database-error"
## Merge Strategie
- Maak gebruik van een pull request voor het mergen. Hierbij wordt gebruik gemaakt van merge commits
- Bij iedere pull request moet een reviewer aangewezen worden (als er een reviewer aanwezig is die dit mogelijk maakt) die de request eerst reviewed en feedback geeft (indien nodig). Als de reviewer de pull request accepteert mag er pas gemerged worden.
- Indien er geen reviewer aanwezig is die de merge kan reviewen mag alles direct gemerged worden nadat de workflows alle tests hebben afgerond.
## Release Strategie
- Release branches worden, tot de volgende major release (bijvoorbeeld van v0.5 -> v0.6), niet verwijdert nadat deze gemerged is naar main. Indien er wat fout gaat kan dan worden teruggevallen op de release branch
- Bij releases wordt gebruikt gemaakt van tags. Deze tags bevatten het versienummer: "v0.5"
