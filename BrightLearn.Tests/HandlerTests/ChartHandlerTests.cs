using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Models.SatisticModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightLearn.Tests.HandlerTests
{
    [TestClass]
    public class ChartHandlerTests
    {
        ChartHandler handler = new ChartHandler();
        GameHandler _gameHandler = new GameHandler();
        
        [TestMethod]
        public void test_UpdateValues_normal()
        {
            foreach (Game game in _gameHandler.GetAllGames())
            {
                handler.UpdateChartValues(game.ID);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen game gevonden met de ID : `999`")]
        public void test_UpdateValues_WrongID()
        {
            handler.UpdateChartValues(999);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "GameID kan niet 0 zijn!")]
        public void test_UpdateValues_ID_0()
        {
            handler.UpdateChartValues(0);
        }
        

        [TestMethod]
        [ExpectedException(typeof(Exception), "GameID kan niet 0 zijn!")]
        public void test_GetChartValues_ID_0()
        {
            handler.GetChartValues(0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen game gevonden met de ID : `999`")]
        public void test_GetChartValues_WrongID()
        {
            handler.GetChartValues(0, 999);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen chart gevonden met de ID : `999`")]
        public void test_GetChartValues_Wrong_Chart_ID()
        {
            handler.GetChartValues(999, 1);
        }
        

        [TestMethod]
        [ExpectedException(typeof(Exception), "GameID kan niet 0 zijn!")]
        public void test_GetChart_ID_0()
        {
            handler.GetChart("Aantal bonussen", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen game gevonden met de ID : `999`")]
        public void test_GetChart_WrongID()
        {
            handler.GetChart("Aantal bonussen", 999);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen chart gevonden met de naam : `WRONG`")]
        public void test_GetChart_Wrong_Chart_ID()
        {
            handler.GetChart("WRONG", 1);
        }
        

        [TestMethod]
        [ExpectedException(typeof(Exception), "GameID kan niet 0 zijn!")]
        public void test_GetGameCharts_ID_0()
        {
            handler.GetGameCharts(0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Er is geen game gevonden met de ID : `999`")]
        public void test_GetGameCharts_WrongID()
        {
            handler.GetGameCharts(999);
        }

    }
}
