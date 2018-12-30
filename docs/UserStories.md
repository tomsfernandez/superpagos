# Superpagos User Stories

## US-01: Registro

**Como** usuario no registrado \
**Quiero** registrarme en Superpagos \
**Para** usar la aplicación

**Aclaraciones**: los datos a pedir son: nombre, email y contraseña. La dirección de email no podrá repetirse entre usuarios
**UATs**
1. UAT - 01: Registro exitoso
   - El usuario no registrados solicita registrarse en la plataforma
   - El usuario entra los datos requeridos
   - El sistema verifica los datos ingresados y registra al usuario
2. UAT - 02: Registro fallido por email inválido
   - El usuario no registrados solicita registrarse en la plataforma
   - El usuario entra los datos requeridos usando un email inválido
   - El sistema detecta que el email no es válido y notifica al usuario
3. UAT - 03: Registro fallido por email repetido
   - El usuario no registrados solicita registrarse en la plataforma
   - El usuario entra los datos requeridos usando un email repetido
   - El sistema detecta que el email no es válido y notifica al usuario
4. UAT - 04: Registro fallido por falta de datos
   - El usuario no registrados solicita registrarse en la plataforma
   - El usuario no entra al menos uno de los datos pedidos
   - El sistema detecta que faltan datos y notifica al usuario

## US-02: Recuperación de Contraseña

**Como** usuario registrado \
**Quiero** recuperar la contraseña de mi cuenta \
**Para** acceder a mi cuenta

**Aclaraciones**: los datos a pedir son: nombre, email y contraseña. La dirección de email no podrá repetirse entre usuarios
**UATs**
1. UAT - 01: Registro exitoso
   - El usuario no registrados solicita registrarse en la plataforma
   - El usuario entra los datos requeridos
   - El sistema verifica los datos ingresados y registra al usuario
2. UAT - 02: Registro fallido por email inválido
   - El usuario no registrados solicita registrarse en la plataforma
   - El usuario entra los datos requeridos usando un email inválido
   - El sistema detecta que el email no es válido y notifica al usuario
3. UAT - 03: Registro fallido por email repetido
   - El usuario no registrados solicita registrarse en la plataforma
   - El usuario entra los datos requeridos usando un email repetido
   - El sistema detecta que el email no es válido y notifica al usuario
4. UAT - 04: Registro fallido por falta de datos
   - El usuario no registrados solicita registrarse en la plataforma
   - El usuario no entra al menos uno de los datos pedidos
   - El sistema detecta que faltan datos y notifica al usuario.
  
## US-03: Login a Superpagos

**Como** usuario registrado /
**Quiero** entrar a mi cuenta de Superpagos /
**Para** usar la aplicación

**Aclaraciones**: los datos necesarios para hacer login son email y contraseña
**UATs**
1. UAT - 01: Login exitoso
   - El usuario registrado solicita hacer login
   - El usuario registrado ingresa los datos necesarios para ingresar.
   - El sistema verifica los datos y deja al usuario acceder a la aplicación
2. UAT - 02: Login erroneo por contraseña inválida
   - El usuario registrado solicita hacer login
   - El usuario registrado ingresa el email de su cuenta pero una contraseña que no corresponde.
   - El sistema verifica los datos y notifica al usuario que las credenciales son inválidas.
3. UAT - 03: Login erroneo por email no existente
   - El usuario registrado solicita hacer login
   - El usuario registrado ingresa un email que no corresponde a ningún usuario.
   - El sistema verifica los datos y notifica al usuario que las credenciales son inválidas.

## US-04: Visualización y adminsitración de un medio de pago

**Como** usuario logueado \
**Quiero** asociar un medio de pago a mi cuenta \
**Para** transferir dinero a otro usuario por ese medio. \

**Aclaraciones**: los medios de pago a elegir deben estar habilitados por Superpagos. La acreditación de la identidad del usuario en el medio de pago es responsabilidad del medio de pago elegido. \
El usuario podrá cancelar la acreditación con el medio de pago si lo desea. \
Una vez acreditado en ese medio de pago, el usuario debe ingresar un nombre y descripción para esa asociación. De esa manera, el usuario podrá asociar varias cuentas del mismo medio de pago si este lo soporta. \
El nombre ingresado no puede estar en cualquier otra asociación del usuario. \
Por cada medio de pago asociado se debe mostrar el nombre dado por el usuario, la descripción, la fecha de asociación, el nombre de proveedor y su logo. Al lado de cada medio de pago debe haber una opción para dar de baja el medio de pago en la cuenta.

**UATs**
1. UAT - 01: Asociación exitosa
   - El sistema muestra los medios de pago disponibles para asociar a la cuenta.
   - El usuario elige un medio de pago para asociar a su cuenta.
   - El sistema abre una ventana delegando la acreditación del usuario con el medio de pago a ese medio de pago.
   - El usuario se acredita correctamente en el medio de pago.
   - El usuario entra un nombre que no pertenece a ninguna otra asociación y una descripción para esa asociación.
2. UAT - 02: Cancelación de pedido de asociación
   - El sistema muestra los medios de pago disponibles para asociar a la cuenta.
   - El usuario elige un medio de pago para asociar a su cuenta.
   - El sistema abre una ventana delegando la acreditación del usuario con el medio de pago a ese medio de pago.
   - El usuario cancela la acreditación con el medio de pago.
3. UAT - 03: Ingreso de nombre repetido
   - El sistema muestra los medios de pago disponibles para asociar a la cuenta.
   - El usuario elige un medio de pago para asociar a su cuenta.
   - El sistema abre una ventana delegando la acreditación del usuario con el medio de pago a ese medio de pago.
   - El usuario se acredita correctamente en el medio de pago.
   - El usuario entra un nombre pertenece a otra asociación y una descripción para esa asociación.
   - El sistema le notifica al usuario que ese nombre ya está en uso.
4. UAT - 04: Baja de medio de pago
   - El usuario selecciona un medio de pago para dar de baja.
   - El usuario confirma el pedido de baja.
   - El sistema dá de baja el medio de pago de la cuenta del usuario.

## US - 05: Realización de un pago
    
**Como** usuario logueado \
**Quiero** realizar un pago a un medio de pago de otra cuenta \
**Para** transferir dinero a otro usuario.

**Aclaraciones**: el usuario deberá seleccionar el medio de pago origen, la cuenta al que se le quiere hace el pago y el monto a pagar.

**UATs**
1. UAT - 01: Pago exitoso
   - El usuario ingresa los datos necesarios para realizar el pago.
   - El usuario confirma el pago.
   - El sistema notifica al proveedor origen que debe debitar la cantidad de dinero ingresada por el usuario origen para el medio de pago elegido.
   - El sistema notifica al proveedor destino que debe dar la cantidad de dinero ingresada por el usuario al usuario destino para el medio de pago elegido.
   - El sistema muestra un mensaje confirmando que el pago fue exitoso.
2. UAT - 02: Falta de fondos
   - El usuario ingresa los datos necesarios para realizar el pago.
   - El usuario confirma el pago.
   - El proveedor del medio de pago notifica al sistema de que no se poseen los fondos suficientes para realizar esa transacción.
   - El sistema notifica al usuario del error.
3. UAT - 03: Usuario origen y destino son iguales
   - El usuario ingresa los datos necesarios para realizar el pago, ingresando a sí mismo a uno de sus medios de pago como destino.
   - El sistema verifica que el origen y el destino son iguales y no permite la confirmación
   - El sistema notifica al usuario del suceso.
4. UAT - 04: Datos faltantes
   - El usuario ingresa algunos de los datos necesarios para realizar un pago.
   - El sistema detecta que no todos los campos fueron ingresados.
   - El sistema notifica al usuario del suceso y no permite la confirmación del pago.

## US - 06: Consulta de transacciones realizadas

**Como** usuario logueado
**Quiero** consultar mis transacciones realizadas
**Para** ver las operaciones realizadas desde o hacia mi cuenta.

**Aclaraciones**: cada entrada consistirá de los campos: fecha de la transacción, descripción de la transacción, monto y medio de pago. \
La información debe estar ordenada por fecha empezando desde la más reciente. \

**UATs**
1. UAT - 01: El usuario hace un pago.
   - El usuario hace una transacción.
   - El usuario busca la última transacción hecha
   - El usuario debe poder visualizar todos los campos de la transacción.

2. UAT - 02: El usuario recibe el dinero de un pago. 
   - Idem UAT-01 pero el usuario no hace la transacción.

## US-07: Logout

**Como** usuario logueado
**Quiero** desloguearme de la aplicación
**Para** que otros no puedan acceder a mi cuenta.

**UATs**
1. UAT - 01: Logout exitoso
   - El usuario le indica al sistema que quiere hacer un logout
   - El sistema redirige al usuario hacia la landing page.

## US-08: Seguridad en contraseña

**Como** security lead \
**Quiero** hashear los passwords con PBKDF2 \
**Para** evitar conocer las passwords de los usuarios

## US-09: Seguridad de contraseña de usuario en API

**Como** security lead \
**Quiero** que no se devuelva un usuario con su password encriptada \
**Para** no divulgar la encriptación del password del usuario

## US-10: Autenticación y Autorización

**Como** usuario de Superpagos \
**Quiero** que mi información sea privada a mí \
**Para** que solo yo pueda efectuar transacciones y ver mi información
