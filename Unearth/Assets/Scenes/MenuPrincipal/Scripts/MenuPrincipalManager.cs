using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{

    public GameObject MenuPrincipal;
    public GameObject TamanhoMapa;
    public GameObject Configuracoes;

    float sensibilidade = 1.0f;

    

    public void Jogar(){
        MenuPrincipal.SetActive(false);
        TamanhoMapa.SetActive(true);
    }

    public void Aplicar(){
        PlayerPrefs.SetFloat("sensibilidade", sensibilidade);
        Config();
    }

    public void Config(){
        MenuPrincipal.SetActive(!MenuPrincipal.activeSelf);
        Configuracoes.SetActive(!Configuracoes.activeSelf);
    }


    public void Voltar(){
        TamanhoMapa.SetActive(false);
        MenuPrincipal.SetActive(true);
    }

    public void Tam(string selecao){
        PlayerPrefs.SetString("tamMapa", selecao);
        SceneManager.LoadScene(1);
    }

    public void Grande(){
        Tam("big");
    }

    public void Medio(){
        Tam("medium");
    }

    public void Pequeno(){
        Tam("small");
    }




    public void SairJogo(){
        Debug.Log("Saindo do jogo");
        Application.Quit();
    }

}
