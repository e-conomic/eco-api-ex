package com.economic.example.SoapClient;

import com.e_conomic.ArrayOfOrderHandle;
import com.e_conomic.EconomicWebService;
import com.e_conomic.EconomicWebServiceSoap;
import com.e_conomic.OrderHandle;

import javax.xml.ws.BindingProvider;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.List;
import java.util.Map;
import java.util.HashMap;
import java.util.Collections;

public class SoapClient {
    public static void main(String[] args) {
        int agreementNumber = 000000;
        String userName = "XXX";
        String password = "********";

        URL apiUrl = null;
        try {
            apiUrl = new URL("https://api.e-conomic.com/secure/api1/EconomicWebService.asmx?wsdl");
        } catch (MalformedURLException e1) {
            e1.printStackTrace();
        }

        EconomicWebService service = new EconomicWebService(apiUrl);
        EconomicWebServiceSoap session = service.getEconomicWebServiceSoap();
        BindingProvider bindingProvider = ((BindingProvider)session);

        //Enable session cookies
        bindingProvider.getRequestContext().put(BindingProvider.SESSION_MAINTAIN_PROPERTY,true);

        //Add X-EconomicAppIdentifier header
        Map<String, List<String>> headers = new HashMap<String, List<String>>();
        headers.put("X-EconomicAppIdentifier", Collections.singletonList("X-EconomicAppIdentifier: MyCoolIntegration/1.1 (http://example.com/MyCoolIntegration/; MyCoolIntegration@example.com) BasedOnSuperLib/1.4"));
        bindingProvider.getRequestContext().put("javax.xml.ws.http.request.headers", headers);

        try {
            session.connect(agreementNumber, userName, password);
        } catch (Exception e) {
            System.out.println("Connection error! \n" + e);
            return;
        }
        System.out.println("Connection successful!");

        try {
            session.verifyXEconomicAppIdentifier();
        } catch (Exception e) {
            System.out.println("X-EconomicAppIdentifier not valid! \n" + e);
            return;
        }
        System.out.println("X-EconomicAppIdentifier is valid!");



        ArrayOfOrderHandle orders = session.orderGetAll();
        List<OrderHandle> oh = orders.getOrderHandle();
        System.out.println(oh.size()+" orders");

        session.disconnect();

    }
}