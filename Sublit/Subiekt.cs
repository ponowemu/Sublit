﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sublit
{

    class Subiekt
    {
        private static InsERT.Subiekt sub;
        [STAThread]
        public void Connect()
        {
            try
            {
                // Utworzenie obiektu GT
                InsERT.GT gt = new InsERT.GT();

                gt.Produkt = InsERT.ProduktEnum.gtaProduktSubiekt;
                gt.Serwer = "25.22.236.167\\INSERTGT";
                gt.Baza = "A_M_Consultants";
                gt.Autentykacja = InsERT.AutentykacjaEnum.gtaAutentykacjaMieszana;
                gt.Uzytkownik = "sa";
                gt.UzytkownikHaslo = "";
                gt.Operator = "Ferfet Mikołaj";
                gt.OperatorHaslo = "Sp123!@#";

                // Uruchomienie Subiekta GT
                sub = (InsERT.Subiekt)gt.Uruchom((Int32)InsERT.UruchomDopasujEnum.gtaUruchomDopasuj, (Int32)InsERT.UruchomEnum.gtaUruchomNieArchiwizujPrzyZamykaniu);
                sub.Okno.Widoczne = false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
