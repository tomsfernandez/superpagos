describe('Associate Payment Method with server', () => {

  const credentials = {
    name: "Barney Ullrich", 
    email: "Rodrick_Russel81@yahoo.com", 
    password: "OAsudlZIUTckj"
  };

  beforeEach(() => {
    cy.server();
    cy.visit('/');
    cy.register(credentials);
    cy.login(credentials);
    cy.addProvider();
  });
  
  afterEach(() => {
    cy.removeAllMethodsFromUser(credentials.email);
    cy.removeUser(credentials.email);
    cy.removeProvider();
  });

  it('associates payment succesfully and redirects to method view', () => {
    cy.route(`${Cypress.env('EXTERNAL_API')}/api/PaymentMethods`).as('getMethods');
    cy.visit('home/methodRegister?code=DEMO&token=aToken');
    cy.wait('@getMethods');
    cy.url().should('include','home/methods');
  });
});