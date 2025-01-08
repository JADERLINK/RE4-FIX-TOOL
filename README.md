# RE4-FIX-TOOL
Extract and repack RE4 FIX files (RE4 UHD/PS4/NS)

**Translate from Portuguese Brazil**

Programa destinado a extrair e reempacotar arquivos .fix das versões UHD, PS4 e NS;
<br> Ao extrair será gerado um arquivo de extenção .idxfix, ele será usado para o repack.

**update: 1.0.2**
<br>Corrigido bug ao extrair arquivos de imagens com 0 de tamanho.
<br>Agora, ao arrastar arquivos sobre o programa, ele vai ficar aberto após extrair/reempacotar.
<br>Os arquivos bat funcionam iguais a antes, mas agora adicionei mais um parâmetro neles.

## Extract

Exemplo:
<br>*RE4_FIX_TOOL.exe "NowLoading.fix"*

* Vai gerar um arquivo de nome "NowLoading.idxfix";
* Vai criar uma pasta de nome "NowLoading";
* Na pasta vão conter as texturas, nomeadas numericamente com 4 dígitos. Ex: 0000.dds;

## Repack

Exemplo:
<br>*RE4_FIX_TOOL.exe "NowLoading.idxfix"*

* Vai ler as imagens da pasta "NowLoading";
* A quantidade é definida pela numeração (então não deixe imagens faltando no meio);
* O nome do arquivo gerado vai ser "NowLoading.fix";

## Avisos:
A versão de NS e UHD só aceita imagens no formato DDS;
<br>A versão de PS4 só aceita imagens no formato GNF;
<br>Nota: Não testei o jogo com imagem TGA, mas o programa aceita fazer repack com esse formato de imagem também;

**At.te: JADERLINK**
<br>2025-01-08