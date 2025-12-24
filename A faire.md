Modules à créer :
* Core
* CRM
* Facturation
* Comptabilité
* Gestion des admissions
* Plannification du personnel

MCD pour Gestion des admissions :
Individu (0,n) -> (1,1) Resident (1,1) <- Etablissement
Etablissement (0,n) -> (1,1) Chambre
Individu (0,n) -> (1,2) Contrat (0,n) -> (1,1) Avenant
Résident (0,n) -> (1,1) Sejour (0,1) <- (0,n) Chambre
                               (0,1) <- (0,n) Contrat/Avenant


Répartition 
Core (Entités partagées)
├── Individu
├── Resident
├── Etablissement
├── Chambre
├── Contract (Core: signature / id, dates, statuts)
└── ValueObjects

CRM Module
├── Prospect
├── Opportunity
└── Contract (Extension: conditions commerciales, workflow)

Gestion Admin Module
├── Sejour
├── Avenant
└── Contract (Extension: avenants opérationnels, affectation à Resident)                               