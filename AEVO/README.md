
# Task - Prática

Olá! 👋
Primeiramente, parabéns por ter chegado até aqui! Essa tem sido uma jornada seletiva de altíssimo nível.E como funcionará a Task Prática?
 
Dividimos em duas etapas, para que você possa aplicar seus conhecimentos e práticas, em cada uma das frentes de desenvolvimento.
O desafio consiste em uma pequena implementação para avaliarmos seu conhecimento em Back-End (.NET, C#) e Front-End (HTML5, CSS, JavaScript e Angular)

Para realizá-lo, você deverá dar um fork neste repositório e depois cloná-lo em alguma pasta de sua preferência, na máquina que estiver realizando o teste.
Crie um branch com seu nome a partir da master e, quando finalizar todo o desenvolvimento, você deverá enviar um pull-request com sua versão.

Caso precise consultar algum material de apoio, recomendamos:<br>
Para o back: https://www.macoratti.net/19/10/ang7_apinc1.htm<br>
Para o front: https://www.youtube.com/@loianegroner <br>

# O Desafio
## Back-End/.NET
A primeira etapa será o desenvolvimento back-end!

Descrição:

O Objetivo dessa etapa é criar apis para um CRUD de agendamento online onde um usuário pode fazer um ou mais agendamento em uma sala que esteja com horario disponivel. Para auxiliarmos e não tornar o desafio muito extenso, iremos fornecer nesse repositorio uma implementação basica para as rotas de CRUD de usuário e de salas. lembressse que cada usuário tem particularidades de idioma e localidade, que devem ser levadas em consideração no momento de agendamento de cada sala.

**Obrigatorio**  - Você deverá desenvolver as seguintes rotas em .Net Core(Versão de sua preferencia):

    /agendamentos/salas/{id} - [GET] - Deve Retornar todos os agendamentos da sala de id especificado
    /agendamentos/salas/{id} - [POST] - Deve cadastrar um novo agendamentos da sala de id especificado
    /agendamentos/{id} - [PUT] - Deve atualizar o agendamento de id especificado;
    /agendamentos/{id} - [DELETE] - Deve deletar o agendamento de id especificado;
    /agendamentos - [GET] - Deve Retornar todos os agendamentos do usuario solicitante;

Você pode utilizar um banco de dados local SQL Server para a persistência dos dados.

## Front-End /Angular
Para a segunda etapa do teste, você deverá desenvolver uma SPA (Single Page Application) utilizando Angular. Nela, deverá ser possível:

**Obrigatorio**  - Você deverá desenvolver no minimo uma tela com as seguintes funcionalidades:

- Ver todos os agendamentos de um usuário;
- Ver todos os agendamentos de uma sala;
- Editar um agendamento feito;
- Excluir um agendamento feito;
- Criar um novo agendamento;
- Alterar o idioma da pagina;

### Observações importantes:
Você pode desenvolver o front na sua versão do Angular de preferência, se atentando para utilizar uma versão superior ou igual a 6.<br>
Você pode modelar as classes da forma que achar mais conveniente para o desenvolvimento.<br>
Você pode construir o layout do front da forma que achar melhor, considere a experiencia do usuário.<br>
Você pode usar ferramentas de automação, mas deverá informar o uso completo para funcionamento do desafio.<br><br>

Serão considerados pontos positivos, porém não são obrigatórios: 

 1. Diferencial - Escrever testes unitarios para os endpoints existentes;
 2. Diferencial - Garantir que os novos endpoints de agendamento passem nos testes pre-definidos;
 3. Diferencial - Utilização de documentação para o mini projeto;
 4. Diferencial - Publicação do projeto em algum ambiente online;
 5. Diferencial - Design patterns e rotinas para testes;
 6. Diferencial - Boas Práticas de orientação a objetos;

<br>

Qualquer problema ou dificuldade com o repositório, você pode entrar em contato conosco pelos e-mails, carlos.pedroni@aevo.com.br ou rh@aevo.com.br para que possamos sanar todas as dúvidas!
<br><br>
Estamos sempre em busca de melhoria. Por isso, caso tenha alguma sugestão, fique à vontade para compartilhar conosco! Boa sorte! 💛

