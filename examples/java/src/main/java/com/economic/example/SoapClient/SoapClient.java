package com.economic.example.SoapClient;

import com.e_conomic.ArrayOfOrderHandle;
import com.e_conomic.EconomicWebService;
import com.e_conomic.EconomicWebServiceSoap;
import com.e_conomic.OrderHandle;

import javax.xml.ws.BindingProvider;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.List;

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

        ((BindingProvider)session).getRequestContext().put(BindingProvider.SESSION_MAINTAIN_PROPERTY,true);

        try {
            session.connect(agreementNumber, userName, password);
        } catch (Exception e) {
            System.out.println("Connection error! " + e);
        }
        System.out.println("Connection successful!");


        ArrayOfOrderHandle orders = session.orderGetAll();
        List<OrderHandle> oh = orders.getOrderHandle();
        System.out.println(oh.size()+" orders");

        session.disconnect();

    }
}