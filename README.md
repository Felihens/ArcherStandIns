# Archer Stand-ins

**[English](#english) · [Português](#português)**

---

## English

Playing with a friend who uses **Vikings Archer**, but you don't have it? Then their bow is invisible in their hands, and their arrows and special shots fly unseen. This mod fixes that on your side. Each Vikings Archer bow and arrow is shown as a look-alike bow or arrow from the base game.

**Test version (0.1.0).** It has not been tested in a real game yet. Please report what you see.

### Who installs it

- **Players who do NOT have Vikings Archer.** That's who it's for.
- Players who have Vikings Archer don't need it. If they install it anyway, it turns itself off.
- The server doesn't need it.

### What you see

| Vikings Archer bow | Shown as |
|---|---|
| Meadows | Crude bow |
| Black Forest | Finewood bow |
| Swamp | Huntsman bow |
| Mountain and Plains | Draugr fang |
| Mistlands | Spinesnap |
| Ashlands | Ashlands bow |

Barrage versions show the same bow. Arrows in flight, including Multishot and Barrage shots, are shown as bronze (Black Forest), iron (Swamp), silver (Mountain), needle (Plains), carapace (Mistlands) or charred (Ashlands) arrows.

### Limits

- Only the look changes. Damage and hits come from your friend's game, as always.
- Vikings Archer items dropped on the ground stay invisible for you, and you can't pick them up. This stops you from holding an item your game can't use.
- Quivers and other Vikings Archer gear don't show.

### Not part of Vikings Archer

This is an unofficial companion mod. It is not made by or affiliated with the author of Vikings Archer. It contains no files, models, textures or code from Vikings Archer. It only knows the item names, and it builds every stand-in from the base game's own bows and arrows.

### Requirements

- [BepInExPack Valheim](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/)

### Installation

**r2modman / Thunderstore Mod Manager:** to install a downloaded zip, go to Settings → *Import local mod*.

**Nexus Mods / Vortex:** also on [Nexus Mods](https://www.nexusmods.com/valheim/mods/4927).

**Manual:**
1. Extract the zip.
2. Copy the `plugins/ArcherStandIns` folder into `Valheim/BepInEx/plugins/`.
   Result: `Valheim/BepInEx/plugins/ArcherStandIns/ArcherStandIns.dll`.
3. Launch the game.

To update, delete the old `ArcherStandIns` folder before copying the new one.

### Configuration

Press **F1** in game (requires Configuration Manager) or edit `BepInEx/config/felih.ArcherStandIns.cfg`.

| Option | Default | What it does |
|---|---|---|
| Enabled | on | Show stand-in bows and arrows. Does nothing when you have Vikings Archer yourself. |
| Bows | see above | Which game bow stands in for each Vikings Archer bow, as `name=GameBow` pairs. |
| Arrows | see above | Which game arrow stands in for each biome's arrows in flight, as `biome=GameArrow` pairs. |
| DetailedLog | off | Writes details to the BepInEx log (for bug reports). |

---

## Português

Joga com um amigo que usa o **Vikings Archer**, mas você não tem esse mod? Então o arco some da mão dele, e as flechas e os ataques especiais voam invisíveis. Este mod resolve isso no seu jogo. Cada arco e flecha do Vikings Archer aparece como um arco ou flecha parecido do próprio jogo.

**Versão de teste (0.1.0).** Ainda não foi testado num jogo de verdade. Conte o que você vê.

### Quem instala

- **Quem NÃO tem o Vikings Archer.** É para essas pessoas.
- Quem tem o Vikings Archer não precisa. Se instalar mesmo assim, ele se desliga sozinho.
- O servidor não precisa.

### O que você vê

| Arco do Vikings Archer | Aparece como |
|---|---|
| Prado | Arco rústico |
| Floresta Negra | Arco de madeira nobre |
| Pântano | Arco do caçador |
| Montanha e Planície | Presa de draugr |
| Névoas | Quebra-espinha |
| Cinzas | Arco das Cinzas |

As versões Barrage mostram o mesmo arco. As flechas no ar, inclusive as do Multishot e do Barrage, aparecem como flechas de bronze (Floresta Negra), ferro (Pântano), prata (Montanha), agulha (Planície), carapaça (Névoas) ou carbonizada (Cinzas).

### Limites

- Só muda o visual. O dano e os acertos vêm do jogo do seu amigo, como sempre.
- Itens do Vikings Archer largados no chão continuam invisíveis para você, e não dá para pegá-los. Assim você não fica com um item que o seu jogo não sabe usar.
- Aljavas e outros equipamentos do Vikings Archer não aparecem.

### Não faz parte do Vikings Archer

Este é um mod de apoio não oficial. Não é do autor do Vikings Archer nem tem ligação com ele. Não contém arquivos, modelos, texturas nem código do Vikings Archer. Só conhece os nomes dos itens e monta tudo com os arcos e flechas do próprio jogo.

### Requisitos

- [BepInExPack Valheim](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/)

### Instalação

**r2modman / Thunderstore Mod Manager:** para instalar um zip baixado, use Configurações → *Import local mod*.

**Nexus Mods / Vortex:** também está no [Nexus Mods](https://www.nexusmods.com/valheim/mods/4927).

**À mão:**
1. Extraia o zip.
2. Copie a pasta `plugins/ArcherStandIns` para dentro de `Valheim/BepInEx/plugins/`.
   Resultado: `Valheim/BepInEx/plugins/ArcherStandIns/ArcherStandIns.dll`.
3. Abra o jogo.

Para atualizar, apague a pasta `ArcherStandIns` antiga antes de copiar a nova.

### Configuração

Aperte **F1** no jogo (precisa do Configuration Manager) ou edite `BepInEx/config/felih.ArcherStandIns.cfg`.

| Opção | Padrão | O que faz |
|---|---|---|
| Enabled | ligado | Mostra os arcos e flechas substitutos. Não faz nada se você tiver o Vikings Archer. |
| Bows | veja acima | Qual arco do jogo substitui cada arco do Vikings Archer, em pares `nome=ArcoDoJogo`. |
| Arrows | veja acima | Qual flecha do jogo substitui as flechas de cada bioma no ar, em pares `bioma=FlechaDoJogo`. |
| DetailedLog | desligado | Escreve detalhes no log do BepInEx (para relatar bugs). |
