# Cosas a considerar

El usuario interactua por frontend (Simple, mínimo, NO IMPORTA QUE SEA FEO.)
Testeable sobre http -> testear navegando. Son pocos casos de uso, se debería testear todos los happy pass. 
También se podría hacer un test de stress o de carga a la parte Web o también en la API, pero HACERLO SOBRE UNO DE ELLOS AL MENOS SI O SI.

Login: básico, password, registración, recuperar contraseña, etc. 

Con un nodo esta bien. Podemos usar en nuestras compus para levantar un test de carga, de stress y probarlo. 

Test
GUI
Stress
Load
Unitarios
Integración
Smoke

Tenemos una sección con diferentes proveedores
Visa: esta tendrá un API entonces tenemos que definir como ejercicio de diseño un API definida por superpagos (además de la API que utiliza la Web) para que los demás proveedores IMPLEMENTEN para que se integren a nosotros. Como OAuth (protocolo que garantiza la identidad segura del usuario, la autenticación, un IDENTITY SERVER). TENEMOS QUE HACERLO PARA LOS SERVICIOS Y PARA LAS PERSONAS. Tenemos que diseñar esa interacción entre SUPERPAGOS y el Provider. ES DESEABLE (no obligatorio) que hagamos un test de integración mockeando un servicio externo para probar nosotros el correcto funcinoamiento de la interfaz genérica de la API que vamos a tener que definir. EL WORKFLOW DE MERCADO PAGO POR EJEMPLO. Se podría meter algún link a la web de superpagos para que laguien se autentique con el usuario y password básico. Entonces una vez loggeado le llega un token o una metadata que si esta autenticado directamente se le redirige a la pagina de autenticación de pagos o la vinculación de pago con el fin de que a esa cuenta de superpago se le vincula una cuenta del proveedor externo. Cuando le llega la solicitud por forontend al tipo para q vincule una cuenta de vis o cualquier proveedor, entonces aca se haría una validación API contra API.
Ejemplo de WorkFlow: un tipo viene proveedor, por ejemplo, “pago mis cuentas”. Viene y dice, “me quiero vincular SuperPagos”. Entonces
Request de vinculación: aca viene algún token (único q identifica al request de vinculación, un token de SOLICITUD de vinculación, se le tiene que pasar un token distinto, no importa la lógica con la que se genera pero esto se termina pasando de vuelta para responderle desde SP, generado por el proveedor.) hacia la Web nuestra. Además pasa el public key o appID. Nosotros somos como un identity server. 
Nosotros validamos que este logueado, validación pertinente, validamos la vinculación etc. Tendriamos que tener un caso de uso, tener una entidad provider que tenga como mínimo el appid. 
Todo OK, y Generamos de token de vinculación, generados por SP únicos en todo el area de SP, que sería otro token distinto al inicial. Este token para que en la próxima request utilice ese token. Necesitariamos guardar que Tal usuario se registro en tal momento con tal token. En algun momento se dice que esta todo bien y llamas al CALLBACK del request, entonces al token que te paso le asignas el token generado por nosotros. Cuando el tipo te devuelva el ok vos guardas.

Caso de uso que piden si o si: VINCULACIÓN. Hay muchos workflows. En cada grupo puede verse un workflow distinto en cada grupo.

El tipo te va a llamar a un webservice para que le otro implemente como callback.

One click action: botones para pagar que cuando haga click afuera, va a irse a superpagos. Es un LINK a la página de pagos. Tiene un token de cuenta a la cual se va a creditar, tu propio id en superpagos. Se tiene que tener algo que dientifique al que esta vendiendo, a la cuenta donde se acredita el dinero. El vendedor tiene la cuenta en superpagos, que es la cuenta que uno que compra. Entonces, Además de tener eso, como no estamos modelando el producto tiene que tener un link, un identificador de la cuenta a la que se va acreditar el dinero y el monto. RESUMEN, se pone un token que identifica a la cuenta vendedora y un monto. Si yo pongo un monto que identifica a la cuenta que puede tener muchos proveedores. Se elige con que cuenta hacer el pago. El tema es ver a que proveedor hacerlo. Una manera de hacerlo es que ese token que pasaste sea uno que identifique el token del super pago y del proveedor, NO USAR EL TOKEN2 PORQUE ES EL TOKEN ENTRE UN PROVEEDOR Y SUPERPAGO, SERVER TO SERVER..

son 2 llamados a 2 proveedores distintos, a uno le decis que le sacas y al otro pagas.

PARA LA PRIMER ENTREGA:
UA test. 
Al menos algo de test del modelo.
Integración continua
User story.

El core del sistema debe estar preferentemente antes del final (parece que se entrega el tp para el final).

En la relación entre proveedor y SP
token privado server to server
token publico
y las cuotas si es que elproveedor acepta.  Esto es de acuerdo al provedor. Esto es parte del workflow de vincualción.CUOTAS FIJAS, eso debe salir de la integración.


Uno de los objetivos cuando uno trabaja con test es que uno expresa el deseo de como quiere utilizar el modelo.

HOOK: se le suele llamar a un web service cuando sos proveedor del servicio y tenes clientes que se te integran y tienen que ser avisados en un evento. Es un evento que tu proveedor del servicio te permite que te vincules. Cuando ocurre este evento en el proveedor del servicio en este caso, SP, y se te vincula los clientes, los providers. 
El hook sincrónico, vos mandas y te quedas esperando a que l otro te responda okay.
Si es asincrónico, vos le decis, me estan queriendo cobrar algo, te responde okay, y luego el otro te llama a tu solicitu de vinculación por otro endpoint.

Docker-compose:
El objetivo de los containers es a los procesos aislarlos. Lo que se hace es aislar procesos, 1 o más procesos. Los containers comienzan aislando un único proceso. Una abstracción un artefacto, es una capacidad del SO para aislar un solo proceso. Se montan arriba de los procesos. Lo que ocurre en verdad, es que estas en el mismo sistema operativo, sin levantar otro kernel que es lo más caro, pero lo estoy aislando, es decir, dandole recursos dedicados, por ejemplo, el disco, cantidad de memoria, la cantidad de cpu, los sockets. Algunos recursos se limitan porque son los que realmente generan decremento en la perfomance de otros procesos, como los semáforos en IPC. Los recursos que se le da a un container son sobre los cuales compiten entre los diferentes procesos. Uno cuando arranca un container tiene un único proceso que va a depender del SO de base, y luego de ese proceso se pueden hacer otros procesos siempre dentro de un mismo container. 
No todos los SO  están preparados entonces lo que ocurre es que en Windows o Mac, en vez de usar docker, se debe usar docker toolbox.
En docker toolbox, se tiene el hardware, arriba, win o mac, arriba virtualbox que es la capa de software, y arriba te levanta un linux que se llama boot to docker, es un tiny core linux, virtualizado, y arriba te corre docker. “Te virtualiza linux para usar docker”, además te isntala una terminal que se comunica mediante un tunel ssh por atrás para comunicarse con docker. ASí no tenes que abrir la interfaz de virtual vbox para usar virtual box por consola. Usas la consola desde tu SO, se comunica por atrás automáticamente con la máquina vritual. Además instala una isnterfaz de usuario.
Otro feature de los frameworks de dockerización es que se tiene un 4to proceso dockerizado, le podes decir que este proceso y otro más para decirle que están en la misma network.

Se nos va a pedir que se tenga acá
Web/API
Api mock del proveedor externo
Tests: tienen que ser ejecutables por consola, se hace un container que ejecute primero los unitarios despues los de stress, despues los de mock, etc. Acá se utiliza el concepto de docker network, de esta forma cuando se hace el docker compose se levantan los containers para la web y api, otro para los mocks externos otro para los tests, por ejemplo, entonces cada uno tiene su nombre que sirve para llamar. Se puede tener todos los tests en un único container o poner cada test en cada container. 
Una técnica comun es usar en los motores de integración continua integración por docker y se debe decir cual es el container de los test, entonces si sale con 0 es okay y si es distinto algo salio mal, entonces por los logs te manda todo lo del container.
LA BUENA PRACTICA ES UN CONTAINER -> UN PROCESO. Si levantas 2 procesos si matas el proceso como principal el container sigue trabajando y cagaste el otro proceso.

HABRIA QUE HACER EL QUICK START DE
DOCKER
DOCKER-COMPOSE


User stories:  la representación de una necesidad del negocio. Es una definición algo que se va a concretar, debe ser concreto, se define según el actor. 
Como ACTOR quiero que NECESIDAD para lograr OBJETIVO.
