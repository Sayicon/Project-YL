/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   token_list.c                                       :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/04/24 18:55:47 by caide-so          #+#    #+#             */
/*   Updated: 2025/04/25 12:22:50 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

void	token_list_init(t_token_list *list)
{
	list->head = NULL;
	list->tail = NULL;
	list->size = 0;
}

t_token	*new_token(t_token_type type, char *value)
{
	t_token	*token;

	token = (t_token *)malloc(sizeof(t_token));
	if (!token)
		return (NULL);
	token->type = type;
	token->value = value;
	token->next = NULL;
	return (token);
}

void	token_list_append(t_token_list *list, t_token *token)
{
	if (list->tail)
		list->tail->next = token;
	else
		list->head = token;
	list->tail = token;
	list->size++;
}

void	token_list_free(t_token_list *list)
{
	t_token	*curr;
	t_token	*next;

	if (!list)
		return ;
	curr = list->head;
	while (curr)
	{
		next = curr->next;
		if (curr->value)
		{
			free(curr->value);
			curr->value = NULL;
		}
		free(curr);
		curr = next;
	}
	free(list);
	list = NULL;
}
