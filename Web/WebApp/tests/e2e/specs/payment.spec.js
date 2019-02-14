import faker from "faker";

const credentials = {
  name: "Barney Ullrich",
  email: "Rodrick_Russel81@yahoo.com",
  password: "OAsudlZIUTckj"
};

describe('Payment Test with Server', () => {

  beforeEach(() => {
    cy.server();
    cy.visit('/login');
    // create two users
    // create providers
    // create payment methods
    cy.removeUser(credentials.email);
    cy.register(credentials);
    cy.login(credentials);
  });

  afterEach(() => {
    // delete payment methods
    // delete users
    // delete providers
    // delete movements
    // delete transactions
    cy.removeUser(credentials.email);
  });

  it('makes payment succesfully and continues to home', () => {
    cy.visit('home/payment/4');
    
  });

  it('an error occurs, message is shown and continues to home', () => {
    
  });
});

describe('Payment UI tests', () => {

  beforeEach(() => {
    cy.server();
    cy.visit('/login');
    // create two users
    // create providers
    // create payment methods
    cy.removeUser(credentials.email);
    cy.register(credentials);
    cy.login(credentials);
  });

  it('attemps several requests before receiving confirmation', () => {

    const transactionId = 11;
    let status = {success: false, failure: false, inProcess: true};
    let succesfullResult = {success: true, failure: false, inProcess: false};
    cy.route('GET', `${Cypress.env('EXTERNAL_API')}/api/PaymentMethods`, 'fixture:methods.json').as('methods');
    cy.route('POST',`${Cypress.env('EXTERNAL_API')}/api/Payment`, transactionId);
    
    cy.route('GET', `${Cypress.env('EXTERNAL_API')}/api/Movements/state/${transactionId}`, status).as('statusCheck');
    cy.visit('home/payment/4');
    cy.get('[data-cy=methods]').select('Visa');
    cy.get('[data-cy=description]').type(faker.lorem.slug());
    cy.get('[data-cy=submit]').click();
    cy.get('[data-cy=loader]').should('be.visible');
    cy.wait('@statusCheck');
    cy.route('GET', `${Cypress.env('EXTERNAL_API')}/api/Movements/state/${transactionId}`, succesfullResult).as('succesfullCheck');
    cy.wait('@succesfullCheck');
    cy.get('[data-cy=payment-success]').should('be.visible');
  });
  
  it('doesnt submit if method is missing', () => {
    cy.route('GET', `${Cypress.env('EXTERNAL_API')}/api/PaymentMethods`, 'fixture:methods.json').as('methods');
    cy.visit('home/payment/4');
    cy.get('[data-cy=description]').type(faker.lorem.slug());
    cy.get('[data-cy=submit]').click();
    cy.get('[data-cy=loader]').should('not.be.visible');
  });

  it('doesnt submit if description is missing', () => {
    cy.route('GET', `${Cypress.env('EXTERNAL_API')}/api/PaymentMethods`, 'fixture:methods.json').as('methods');
    cy.visit('home/payment/4');
    cy.get('[data-cy=methods]').select('Visa');
    cy.get('[data-cy=submit]').click();
    cy.get('[data-cy=loader]').should('not.be.visible');
  });

  afterEach(() => {
    // delete payment methods
    // delete users
    // delete providers
    // delete movements
    // delete transactions
    cy.removeUser(credentials.email);
  });
});