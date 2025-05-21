# Rnnna2

Nous avons utiliser la methode ensemble learning qui utiliser deux modele entrainer sur le DATASET HG14 
Le premier modele MobileNetv2 et le second est MobileNetV3 
  pourquoi ce choix ? 
- en raison de la rapidite et de l'efficacite de ses deux modeles
  Pour l'entrainement de nos deux modele mobileNetv2 on a degeler les 20 derniers couche donc on l'a entrainer sur notre data set pour faire de la features extraction
  et pour le modele MobileNetV3 on l'a utiliser directement comme tel on a changer uniquement la couche du classifier pour reconnaitre uniquement Nos 14 differantes classes

- Pour le esemble learning nous avons utiliser 0.7 des poids de MobileNetv2 et 0.3 des poids de MobileNetV3 

Vous trouverai les NoteBook d'entrainement dans le Dossier Folder-entrainement 

- Pour l'application nous avons mise en place un jeux qui permet de deplacer le joueur a l'aide de la camera et la reconnaissance des geste de la mains 
