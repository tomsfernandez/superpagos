describe('Operations Test', () => {

    const credentials = {
        name: "Barney Ullrich",
        email: "Rodrick_Russel81@yahoo.com",
        password: "OAsudlZIUTckj"
    };
    
    before(() => {
        cy.server();
        cy.removeAllMethodsFromUser(credentials.email);
        cy.removeUser(credentials.email);
        cy.removeProvider();
        cy.register(credentials);
        cy.addProvider();
    });
    
    beforeEach(() => {
        cy.visit('/');
        cy.login(credentials);
    });
    
    afterEach(() => {
        cy.logout();
        cy.removeMovements(credentials.email);
    });

    after(() => {
        cy.removeAllMethodsFromUser(credentials.email);
        cy.removeUser(credentials.email);
        cy.removeProvider();
    });
    
    it('visits the operation\'s page and the movements are displayed in the table', () => {
        cy.addMovements(credentials.email);
        cy.visit('/home');
        cy.get('.k-grid-content tr').should('have.length', 3);
    });

    it('visits the operation\'s page and there is no movement displayed in the table', () => {
        cy.visit('/home');
        cy.get('.k-grid-content tr').should('have.length', 0);
    });
});